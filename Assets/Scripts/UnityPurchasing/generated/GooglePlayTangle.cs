// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("EQWyIkNarHM635i6Nv9VIIqQuGOx8dm77DwYJnA77lh5MvyF+4/bHEkYaprEQtnaVhxFxd0eTTv8KvuH1+9Lpq2LUhV8aYlP5p9H4JVCFUeJO7ibibS/sJM/8T9OtLi4uLy5ugBzbAkzWMTV3XiuHTM0ULP/lwu2j7wU6BMO2dQRRIp2LUIOT0pIQmg+BOCadu8BESnINz094YPjG1JIEV+F7DNoMnqN4t/lRPJWTQ3GeoJ30SL8gk1CoKtP6Jcmu0SKxUVEPIlPj10jm73G2JONwdaD2J1xuda/VSwrJTdjJmkiCP8njdS/WljTxxs507BQGZ7f8DODTsiVSd3u11VNqu87uLa5iTu4s7s7uLi5I6Mufb82p3c+LQZmm0f5hru6uLm4");
        private static int[] order = new int[] { 13,9,4,8,13,8,10,11,13,11,12,12,13,13,14 };
        private static int key = 185;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
