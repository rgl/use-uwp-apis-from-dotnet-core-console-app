using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System;
using Windows.System.UserProfile;

namespace ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // save the current lockscreen.
            using (var f = File.Create(string.Format("lock-screen-{0:yyyyMMddHHmmss}.jpg", DateTime.Now)))
            using (var imageStream = LockScreen.GetImageStream())
            {
                await imageStream.AsStreamForRead().CopyToAsync(f);
            }

            // set the lockscreen.
            var lockScreenImageFile = await StorageFile.GetFileFromPathAsync(Path.GetFullPath("vladstudio_coffee_station_wallpaper.jpg"));
            await LockScreen.SetImageFileAsync(lockScreenImageFile);

            // set the wallpaper.
            // XXX this does not work...
            Console.WriteLine("UserProfilePersonalizationSettings.IsSupported? {0}", UserProfilePersonalizationSettings.IsSupported());
            var wallpaperImageFile = await StorageFile.GetFileFromPathAsync(Path.GetFullPath("vladstudio_cats_wallpaper.jpg"));
            var hasSetWallpaper = await UserProfilePersonalizationSettings.Current.TrySetWallpaperImageAsync(wallpaperImageFile);
            Console.WriteLine("HasSetWallpaper? {0}", hasSetWallpaper);

            // get user properties.
            foreach (var u in await User.FindAllAsync())
            {
                Console.WriteLine("Type={0} AuthenticationStatus={1} NonRoamableId={2}", u.Type, u.AuthenticationStatus, u.NonRoamableId);

                // XXX this does not work...
                // TODO from a normal UWP application, one needs to declare the uesrAccountInformation capability like in
                //          https://github.com/Microsoft/Windows-universal-samples/blob/master/Samples/UserInfo/cs/Package.appxmanifest
                //          https://docs.microsoft.com/en-us/windows/uwp/packaging/app-capability-declarations
                //      but how to do this on dotnet core?
                var properties = await u.GetPropertiesAsync(
                    new[]
                    { 
                        KnownUserProperties.DomainName,
                        KnownUserProperties.AccountName,
                        KnownUserProperties.FirstName,
                        KnownUserProperties.LastName,
                        KnownUserProperties.DisplayName,
                        KnownUserProperties.PrincipalName,
                        KnownUserProperties.ProviderName,
                    }
                );
                foreach (var kp in properties)
                {
                    Console.WriteLine("User Property: {0}:{1}={2}", kp.Key, kp.Value?.GetType(), kp.Value);
                }

                // save user profile picture.
                // XXX this does not work...
                var userPicture = await u.GetPictureAsync(UserPictureSize.Size64x64);
                using (var usePictureStream = await userPicture.OpenReadAsync())
                using (var f = File.Create(string.Format("user-picture-{0:yyyyMMddHHmmss}.jpg", DateTime.Now)))
                {
                    await usePictureStream.AsStreamForRead().CopyToAsync(f);
                }
            }
        }
    }
}
