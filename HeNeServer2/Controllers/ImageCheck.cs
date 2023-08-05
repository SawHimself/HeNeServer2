namespace HeNeServer2.Controllers
{
    public class ImageCheck
    {
        public static bool test(string file)
        {

            if(file.Contains(".jpg") || file.Contains(".jpeg") || file.Contains("JPEG") || file.Contains(".png"))
                return true;
            else
                return false;
        }
    }
}
