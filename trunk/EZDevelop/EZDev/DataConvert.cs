#region  修订历史

/*
 * 创建时间：2010-4-6
 * 创建人：董永刚
 * 描述：数据转换类，实现一些特殊的数据转换功能
 */

#endregion

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace EZDev
{
    /// <summary>
    /// 存放各种转换操作
    /// </summary>
    public static class DataConvert
    {
        private static readonly string encryptCode = "fK#dQ1+34f%A";

        private static readonly byte[] encryptIV = new byte[]
                                                       {
                                                           0x72, 0xf3, 0xf3, 0xf0, 0x43, 12, 0x97, 0xc6, 0xdb, 0x40, 0x17,
                                                           0xe0, 0x43, 0x7d, 0xd4, 0x22
                                                       };

        /// <summary>
        /// 拼音字符串
        /// </summary>
        private static readonly string[] spellString = new[]
                                                           {
                                                               "a", "ai", "an", "ang", "ao", "ba", "bai", "ban", "bang",
                                                               "bao", "bei", "ben", "beng", "bi", "bian",
                                                               "biao",
                                                               "bie", "bin", "bing", "bo", "bu", "ca", "cai", "can",
                                                               "cang", "cao", "ce", "ceng", "cha", "chai", "chan"
                                                               , "chang",
                                                               "chao", "che", "chen", "cheng", "chi", "chong", "chou",
                                                               "chu", "chuai", "chuan", "chuang", "chui",
                                                               "chun", "chuo", "ci", "cong",
                                                               "cou", "cu", "cuan", "cui", "cun", "cuo", "da", "dai",
                                                               "dan", "dang", "dao", "de", "deng", "di", "dian",
                                                               "diao",
                                                               "die", "ding", "diu", "dong", "dou", "du", "duan", "dui",
                                                               "dun", "duo", "e", "en", "er", "fa", "fan",
                                                               "fang",
                                                               "fei", "fen", "feng", "fo", "fou", "fu", "ga", "gai",
                                                               "gan", "gang", "gao", "ge", "gei", "gen", "geng",
                                                               "gong",
                                                               "gou", "gu", "gua", "guai", "guan", "guang", "gui", "gun"
                                                               , "guo", "ha", "hai", "han", "hang", "hao",
                                                               "he", "hei",
                                                               "hen", "heng", "hong", "hou", "hu", "hua", "huai", "huan"
                                                               , "huang", "hui", "hun", "huo", "ji", "jia",
                                                               "jian", "jiang",
                                                               "jiao", "jie", "jin", "jing", "jiong", "jiu", "ju",
                                                               "juan", "jue", "jun", "ka", "kai", "kan", "kang",
                                                               "kao", "ke",
                                                               "ken", "keng", "kong", "kou", "ku", "kua", "kuai", "kuan"
                                                               , "kuang", "kui", "kun", "kuo", "la", "lai",
                                                               "lan", "lang",
                                                               "lao", "le", "lei", "leng", "li", "lia", "lian", "liang",
                                                               "liao", "lie", "lin", "ling", "liu", "long",
                                                               "lou", "lu",
                                                               "lv", "luan", "lue", "lun", "luo", "ma", "mai", "man",
                                                               "mang", "mao", "me", "mei", "men", "meng", "mi",
                                                               "mian",
                                                               "miao", "mie", "min", "ming", "miu", "mo", "mou", "mu",
                                                               "na", "nai", "nan", "nang", "nao", "ne", "nei",
                                                               "nen",
                                                               "neng", "ni", "nian", "niang", "niao", "nie", "nin",
                                                               "ning", "niu", "nong", "nu", "nv", "nuan", "nue",
                                                               "nuo", "o",
                                                               "ou", "pa", "pai", "pan", "pang", "pao", "pei", "pen",
                                                               "peng", "pi", "pian", "piao", "pie", "pin",
                                                               "ping", "po",
                                                               "pu", "qi", "qia", "qian", "qiang", "qiao", "qie", "qin",
                                                               "qing", "qiong", "qiu", "qu", "quan", "que",
                                                               "qun", "ran",
                                                               "rang", "rao", "re", "ren", "reng", "ri", "rong", "rou",
                                                               "ru", "ruan", "rui", "run", "ruo", "sa", "sai",
                                                               "san",
                                                               "sang", "sao", "se", "sen", "seng", "sha", "shai", "shan"
                                                               , "shang", "shao", "she", "shen", "sheng",
                                                               "shi", "shou", "shu",
                                                               "shua", "shuai", "shuan", "shuang", "shui", "shun",
                                                               "shuo", "si", "song", "sou", "su", "suan", "sui",
                                                               "sun", "suo", "ta",
                                                               "tai", "tan", "tang", "tao", "te", "teng", "ti", "tian",
                                                               "tiao", "tie", "ting", "tong", "tou", "tu",
                                                               "tuan", "tui",
                                                               "tun", "tuo", "wa", "wai", "wan", "wang", "wei", "wen",
                                                               "weng", "wo", "wu", "xi", "xia", "xian", "xiang"
                                                               , "xiao",
                                                               "xie", "xin", "xing", "xiong", "xiu", "xu", "xuan", "xue"
                                                               , "xun", "ya", "yan", "yang", "yao", "ye", "yi"
                                                               , "yin",
                                                               "ying", "yo", "yong", "you", "yu", "yuan", "yue", "yun",
                                                               "za", "zai", "zan", "zang", "zao", "ze", "zei",
                                                               "zen",
                                                               "zeng", "zha", "zhai", "zhan", "zhang", "zhao", "zhe",
                                                               "zhen", "zheng", "zhi", "zhong", "zhou", "zhu",
                                                               "zhua", "zhuai", "zhuan",
                                                               "zhuang", "zhui", "zhun", "zhuo", "zi", "zong", "zou",
                                                               "zu", "zuan", "zui", "zun", "zuo"
                                                           };

        /// <summary>
        /// 拼音值
        /// </summary>
        private static readonly int[] spellvalue = new[]
                                                       {
                                                           -20319, -20317, -20304, -20295, -20292, -20283, -20265,
                                                           -20257, -20242, -20230, -20051, -20036, -20032,
                                                           -20026, -20002, -19990,
                                                           -19986, -19982, -19976, -19805, -19784, -19775, -19774,
                                                           -19763, -19756, -19751, -19746, -19741, -19739,
                                                           -19728, -19725, -19715,
                                                           -19540, -19531, -19525, -19515, -19500, -19484, -19479,
                                                           -19467, -19289, -19288, -19281, -19275, -19270,
                                                           -19263, -19261, -19249,
                                                           -19243, -19242, -19238, -19235, -19227, -19224, -19218,
                                                           -19212, -19038, -19023, -19018, -19006, -19003,
                                                           -18996, -18977, -18961,
                                                           -18952, -18783, -18774, -18773, -18763, -18756, -18741,
                                                           -18735, -18731, -18722, -18710, -18697, -18696,
                                                           -18526, -18518, -18501,
                                                           -18490, -18478, -18463, -18448, -18447, -18446, -18239,
                                                           -18237, -18231, -18220, -18211, -18201, -18184,
                                                           -18183, -18181, -18012,
                                                           -17997, -17988, -17970, -17964, -17961, -17950, -17947,
                                                           -17931, -17928, -17922, -17759, -17752, -17733,
                                                           -17730, -17721, -17703,
                                                           -17701, -17697, -17692, -17683, -17676, -17496, -17487,
                                                           -17482, -17468, -17454, -17433, -17427, -17417,
                                                           -17202, -17185, -16983,
                                                           -16970, -16942, -16915, -16733, -16708, -16706, -16689,
                                                           -16664, -16657, -16647, -16474, -16470, -16465,
                                                           -16459, -16452, -16448,
                                                           -16433, -16429, -16427, -16423, -16419, -16412, -16407,
                                                           -16403, -16401, -16393, -16220, -16216, -16212,
                                                           -16205, -16202, -16187,
                                                           -16180, -16171, -16169, -16158, -16155, -15959, -15958,
                                                           -15944, -15933, -15920, -15915, -15903, -15889,
                                                           -15878, -15707, -15701,
                                                           -15681, -15667, -15661, -15659, -15652, -15640, -15631,
                                                           -15625, -15454, -15448, -15436, -15435, -15419,
                                                           -15416, -15408, -15394,
                                                           -15385, -15377, -15375, -15369, -15363, -15362, -15183,
                                                           -15180, -15165, -15158, -15153, -15150, -15149,
                                                           -15144, -15143, -15141,
                                                           -15140, -15139, -15128, -15121, -15119, -15117, -15110,
                                                           -15109, -14941, -14937, -14933, -14930, -14929,
                                                           -14928, -14926, -14922,
                                                           -14921, -14914, -14908, -14902, -14894, -14889, -14882,
                                                           -14873, -14871, -14857, -14678, -14674, -14670,
                                                           -14668, -14663, -14654,
                                                           -14645, -14630, -14594, -14429, -14407, -14399, -14384,
                                                           -14379, -14368, -14355, -14353, -14345, -14170,
                                                           -14159, -14151, -14149,
                                                           -14145, -14140, -14137, -14135, -14125, -14123, -14122,
                                                           -14112, -14109, -14099, -14097, -14094, -14092,
                                                           -14090, -14087, -14083,
                                                           -13917, -13914, -13910, -13907, -13906, -13905, -13896,
                                                           -13894, -13878, -13870, -13859, -13847, -13831,
                                                           -13658, -13611, -13601,
                                                           -13406, -13404, -13400, -13398, -13395, -13391, -13387,
                                                           -13383, -13367, -13359, -13356, -13343, -13340,
                                                           -13329, -13326, -13318,
                                                           -13147, -13138, -13120, -13107, -13096, -13095, -13091,
                                                           -13076, -13068, -13063, -13060, -12888, -12875,
                                                           -12871, -12860, -12858,
                                                           -12852, -12849, -12838, -12831, -12829, -12812, -12802,
                                                           -12607, -12597, -12594, -12585, -12556, -12359,
                                                           -12346, -12320, -12300,
                                                           -12120, -12099, -12089, -12074, -12067, -12058, -12039,
                                                           -11867, -11861, -11847, -11831, -11798, -11781,
                                                           -11604, -11589, -11536,
                                                           -11358, -11340, -11339, -11324, -11303, -11097, -11077,
                                                           -11067, -11055, -11052, -11045, -11041, -11038,
                                                           -11024, -11020, -11019,
                                                           -11018, -11014, -10838, -10832, -10815, -10800, -10790,
                                                           -10780, -10764, -10587, -10544, -10533, -10519,
                                                           -10331, -10329, -10328,
                                                           -10322, -10315, -10309, -10307, -10296, -10281, -10274,
                                                           -10270, -10262, -10260, -10256, -10254
                                                       };

        /// <summary>
        /// 获取指定汉字字符串对应得拼音字符串(含声母韵母),
        /// 若汉字串含有非汉字字符,如图形符号或ASCII码,则这些非汉字字符将保持不变
        /// </summary>
        /// <param name="chineseString">汉字字符串</param>
        /// <returns>对应得拼音(一律为小写)</returns>
        public static string GetChineseSpell(string chineseString)
        {
            var bytes = new byte[2];
            string str = "";
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            char[] chArray = chineseString.ToCharArray();
            for (int i = 0; i < chArray.Length; i++)
            {
                bytes = Encoding.Default.GetBytes(chArray[i].ToString());
                if (bytes.Length == 1)
                {
                    num = bytes[0];
                }
                else
                {
                    num2 = bytes[0];
                    num3 = bytes[1];
                    num = ((num2 * 0x100) + num3) - 0x10000;
                }
                if ((num > 0) && (num < 160))
                {
                    str = str + chArray[i];
                }
                else
                {
                    for (int j = spellvalue.Length - 1; j >= 0; j--)
                    {
                        if (spellvalue[j] <= num)
                        {
                            str = str + spellString[j];
                            break;
                        }
                    }
                }
            }
            return str;
        }

        /// <summary>
        /// 得到指定加密字符串解密后的字符串(内部算法)
        /// </summary>
        /// <param name="encryptString">要解密的字符串</param>
        /// <returns>解密后的字符串</returns>
        public static string GetDecryptString(string encryptString)
        {
            int num;
            int index;
            string str5;
            string str3 =
                "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890 /=<>?-!&@~%$#^*()_+'\\\"[]{}:;,.|`";
            string str =
                "QAZ1`XzSW{)ax5#([EDt?6Cq=7*RgsFpvb]l<;VB3|,.o4G}'n\\r:Ti$f hY0&Hc8-y2N/%+>_M!u\"~JU^jdI9@mKeLkOwP";
            string str2 =
                "3nrTo4<G+if)( h,zS%58yq[W7&#pvQax}\\$0Hc:AZ_B*-.1bl>V=F`NM?@2'|Rgs]udI9m{\";KeLkOJ~!^UjwPDt/X6CEY";
            string str4 = "";
            for (num = 0; num < encryptString.Length; num++)
            {
                str5 = encryptString.Substring(num, 1);
                index = str2.IndexOf(str5);
                if (index < 0)
                {
                    str4 = str4 + str5;
                }
                else
                {
                    str4 = str4 + str3.Substring(index, 1);
                }
            }
            string str6 = "";
            for (num = 0; num < str4.Length; num++)
            {
                str5 = str4.Substring(num, 1);
                index = str.IndexOf(str5);
                if (index < 0)
                {
                    str6 = str6 + str5;
                }
                else
                {
                    str6 = str6 + str3.Substring(index, 1);
                }
            }
            return str6;
        }

        /// <summary>
        /// 得到指定字符串加密后的字符串(内部算法)
        /// </summary>
        /// <param name="normalString">要加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string GetEncryptString(string normalString)
        {
            int num;
            int index;
            string str5;
            string str3 =
                "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890 /=<>?-!&@~%$#^*()_+'\\\"[]{}:;,.|`";
            string str =
                "QAZ1`XzSW{)ax5#([EDt?6Cq=7*RgsFpvb]l<;VB3|,.o4G}'n\\r:Ti$f hY0&Hc8-y2N/%+>_M!u\"~JU^jdI9@mKeLkOwP";
            string str2 =
                "3nrTo4<G+if)( h,zS%58yq[W7&#pvQax}\\$0Hc:AZ_B*-.1bl>V=F`NM?@2'|Rgs]udI9m{\";KeLkOJ~!^UjwPDt/X6CEY";
            string str4 = "";
            for (num = 0; num < normalString.Length; num++)
            {
                str5 = normalString.Substring(num, 1);
                index = str3.IndexOf(str5);
                if (index < 0)
                {
                    str4 = str4 + str5;
                }
                else
                {
                    str4 = str4 + str.Substring(index, 1);
                }
            }
            string str6 = "";
            for (num = 0; num < str4.Length; num++)
            {
                str5 = str4.Substring(num, 1);
                index = str3.IndexOf(str5);
                if (index < 0)
                {
                    str6 = str6 + str5;
                }
                else
                {
                    str6 = str6 + str2.Substring(index, 1);
                }
            }
            return str6;
        }

        /// <summary>
        /// 得到根据数据库字段字节数截断的指定字符串（从左边开始截断）
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <param name="lenght">数据库字段字节数</param>
        /// <returns>截断后的字符串</returns>
        public static string GetLeftSubstring(string str, int lenght)
        {
            int length = str.Length;
            if ((length + length) <= lenght)
            {
                return str;
            }
            byte[] bytes = Encoding.Default.GetBytes(str);
            if (bytes.GetLength(0) <= lenght)
            {
                return str;
            }
            int count = lenght;
            for (int i = 0; i < lenght; i++)
            {
                if (bytes[i] >= 0x80)
                {
                    i++;
                    if (i >= lenght)
                    {
                        count = lenght - 1;
                        break;
                    }
                    if (bytes[i] < 0x80)
                    {
                        i--;
                    }
                }
            }
            return Encoding.Default.GetString(bytes, 0, count);
        }

        /// <summary>
        /// 得到根据数据库字段字节数截断的指定字符串（从右边开始截断）
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <param name="lenght">数据库字段字节数</param>
        /// <returns>截断后的字符串</returns>
        public static string GetRightSubstring(string str, int lenght)
        {
            int length = str.Length;
            if ((length + length) <= lenght)
            {
                return str;
            }
            byte[] bytes = Encoding.Default.GetBytes(str);
            if (bytes.GetLength(0) <= lenght)
            {
                return str;
            }
            int count = lenght;
            for (int i = 0; i < lenght; i++)
            {
                if (bytes[(bytes.GetLength(0) - i) - 1] >= 0x80)
                {
                    i++;
                    if (i >= lenght)
                    {
                        count = lenght - 1;
                        break;
                    }
                    if (bytes[(bytes.GetLength(0) - i) - 1] < 0x80)
                    {
                        i--;
                    }
                }
            }
            return Encoding.Default.GetString(bytes, bytes.GetLength(0) - count, count);
        }

        /// <summary>
        /// 返回给定汉字串的首字母串,即声母串
        /// 1. 此方法基于汉字的国标汉字库区位编码的有效性，不符合此编码的
        /// 系统此函数无效！
        /// 2. 若汉字串含有非汉字字符,如图形符号或ASCII码,则这些非汉字字符
        /// 将保持不变
        /// </summary>
        /// <param name="chineseChar">给定的汉字串</param>
        /// <returns>给定的汉字串的声母串,一律为小写</returns>
        /// <example>ls_rtn =   getSpellFirstLetter("中华人民共和国")
        /// 将返回 : zhrmghg</example>
        public static string GetSpellFirstLetter(string chineseChar)
        {
            var chArray = new[]
                              {
                                  'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q',
                                  'R', 'S', 'T', 'W', 'X', 'Y', 'Z'
                              };
            var numArray = new[]
                               {
                                   0x641, 0x665, 0x729, 0x81e, 0x8e2, 0x8fe, 0x981, 0xa22, 0xae3, 0xc22, 0xc8c, 0xd90,
                                   0xe33, 0xe8a,
                                   0xe92, 0xf12,
                                   0xfbb, 0xff6, 0x1126, 0x11ce, 0x124c, 0x133d, 0x1481
                               };
            string str =
                "CJWGNSPGCGNE[Y[BTYYZDXYKYGT[JNNJQMBSGZSCYJSYY[PGKBZGY[YWJKGKLJYWKPJQHY[W[DZLSGMRYPYWWCCKZNKYYGTTNJJNYKKZYTCJNMCYLQLYPYQFQRPZSLWBTGKJFYXJWZLTBNCXJJJJTXDTTSQZYCDXXHGCK[PHFFSS[YBGXLPPBYLL[HLXS[ZM[JHSOJNGHDZQYKLGJHSGQZHXQGKEZZWYSCSCJXYEYXADZPMDSSMZJZQJYZC[J[WQJBYZPXGZNZCPWHKXHQKMWFBPBYDTJZZKQHYLYGXFPTYJYYZPSZLFCHMQSHGMXXSXJ[[DCSBBQBEFSJYHXWGZKPYLQBGLDLCCTNMAYDDKSSNGYCSGXLYZAYBNPTSDKDYLHGYMYLCXPY[JNDQJWXQXFYYFJLEJPZRXCCQWQQSBNKYMGPLBMJRQCFLNYMYQMSQYRBCJTHZTQFRXQHXMJJCJLXQGJMSHZKBSWYEMYLTXFSYDSWLYCJQXSJNQBSCTYHBFTDCYZDJWYGHQFRXWCKQKXEBPTLPXJZSRMEBWHJLBJSLYYSMDXLCLQKXLHXJRZJMFQHXHWYWSBHTRXXGLHQHFNM[YKLDYXZPYLGG[MTCFPAJJZYLJTYANJGBJPLQGDZYQYAXBKYSECJSZNSLYZHSXLZCGHPXZHZNYTDSBCJKDLZAYFMYDLEBBGQYZKXGLDNDNYSKJSHDLYXBCGHXYPKDJMMZNGMMCLGWZSZXZJFZNMLZZTHCSYDBDLLSCDDNLKJYKJSYCJLKWHQASDKNHCSGANHDAASHTCPLCPQYBSDMPJLPZJOQLCDHJJYSPRCHN[NNLHLYYQYHWZPTCZGWWMZFFJQQQQYXACLBHKDJXDGMMYDJXZLLSYGXGKJRYWZWYCLZMSSJZLDBYD[FCXYHLXCHYZJQ[[QAGMNYXPFRKSSBJLYXYSYGLNSCMHZWWMNZJJLXXHCHSY[[TTXRYCYXBYHCSMXJSZNPWGPXXTAYBGAJCXLY[DCCWZOCWKCCSBNHCPDYZNFCYYTYCKXKYBSQKKYTQQXFCWCHCYKELZQBSQYJQCCLMTHSYWHMKTLKJLYCXWHEQQHTQH[PQ[QSCFYMNDMGBWHWLGSLLYSDLMLXPTHMJHWLJZYHZJXHTXJLHXRSWLWZJCBXMHZQXSDZPMGFCSGLSXYMJSHXPJXWMYQKSMYPLRTHBXFTPMHYXLCHLHLZYLXGSSSSTCLSLDCLRPBHZHXYYFHB[GDMYCNQQWLQHJJ[YWJZYEJJDHPBLQXTQKWHLCHQXAGTLXLJXMSL[HTZKZJECXJCJNMFBY[SFYWYBJZGNYSDZSQYRSLJPCLPWXSDWEJBJCBCNAYTWGMPAPCLYQPCLZXSBNMSGGFNZJJBZSFZYNDXHPLQKZCZWALSBCCJX[YZGWKYPSGXFZFCDKHJGXDLQFSGDSLQWZKXTMHSBGZMJZRGLYJBPMLMSXLZJQQHZYJCZYDJWBMYKLDDPMJEGXYHYLXHLQYQHKYCWCJMYYXNATJHYCCXZPCQLBZWWYTWBQCMLPMYRJCCCXFPZNZZLJPLXXYZTZLGDLDCKLYRZZGQTGJHHGJLJAXFGFJZSLCFDQZLCLGJDJCSNZLLJPJQDCCLCJXMYZFTSXGCGSBRZXJQQCTZHGYQTJQQLZXJYLYLBCYAMCSTYLPDJBYREGKLZYZHLYSZQLZNWCZCLLWJQJJJKDGJZOLBBZPPGLGHTGZXYGHZMYCNQSYCYHBHGXKAMTXYXNBSKYZZGJZLQJDFCJXDYGJQJJPMGWGJJJPKQSBGBMMCJSSCLPQPDXCDYYKY[CJDDYYGYWRHJRTGZNYQLDKLJSZZGZQZJGDYKSHPZMTLCPWNJAFYZDJCNMWESCYGLBTZCGMSSLLYXQSXSBSJSBBSGGHFJLYPMZJNLYYWDQSHZXTYYWHMZYHYWDBXBTLMSYYYFSXJC[DXXLHJHF[SXZQHFZMZCZTQCXZXRTTDJHNNYZQQMNQDMMG[YDXMJGDHCDYZBFFALLZTDLTFXMXQZDNGWQDBDCZJDXBZGSQQDDJCMBKZFFXMKDMDSYYSZCMLJDSYNSBRSKMKMPCKLGDBQTFZSWTFGGLYPLLJZHGJ[GYPZLTCSMCNBTJBQFKTHBYZGKPBBYMTDSSXTBNPDKLEYCJNYDDYKZDDHQHSDZSCTARLLTKZLGECLLKJLQJAQNBDKKGHPJTZQKSECSHALQFMMGJNLYJBBTMLYZXDCJPLDLPCQDHZYCBZSCZBZMSLJFLKRZJSNFRGJHXPDHYJYBZGDLQCSEZGXLBLGYXTWMABCHECMWYJYZLLJJYHLG[DJLSLYGKDZPZXJYYZLWCXSZFGWYYDLYHCLJSCMBJHBLYZLYCBLYDPDQYSXQZBYTDKYXJY[CNRJMPDJGKLCLJBCTBJDDBBLBLCZQRPPXJCJLZCSHLTOLJNMDDDLNGKAQHQHJGYKHEZNMSHRP[QQJCHGMFPRXHJGDYCHGHLYRZQLCYQJNZSQTKQJYMSZSWLCFQQQXYFGGYPTQWLMCRNFKKFSYYLQBMQAMMMYXCTPSHCPTXXZZSMPHPSHMCLMLDQFYQXSZYYDYJZZHQPDSZGLSTJBCKBXYQZJSGPSXQZQZRQTBDKYXZKHHGFLBCSMDLDGDZDBLZYYCXNNCSYBZBFGLZZXSWMSCCMQNJQSBDQSJTXXMBLTXZCLZSHZCXRQJGJYLXZFJPHYMZQQYDFQJJLZZNZJCDGZYGCTXMZYSCTLKPHTXHTLBJXJLXSCDQXCBBTJFQZFSLTJBTKQBXXJJLJCHCZDBZJDCZJDCPRNPQCJPFCZLCLZXZDMXMPHJSGZGSZZQLYLWTJPFSYASMCJBTZKYCWMYTCSJJLJCQLWZMALBXYFBPNLSFHTGJWEJJXXGLLJSTGSHJQLZFKCGNNNSZFDEQFHBSAQTGYLBXMMYGSZLDYDQMJJRGBJTKGDHGKBLQKBDMBYLXWCXYTTYBKMRTJZXQJBHLMHMJJZMQASLDCYXYQDLQCAFYWYXQHZ";
            string str2 = "";
            byte[] bytes = Encoding.Default.GetBytes(chineseChar);
            for (int i = 0; i < bytes.GetLength(0); i++)
            {
                if (bytes[i] < 0x80)
                {
                    str2 = str2 + Convert.ToChar(bytes[i]);
                    continue;
                }
                int num3 = bytes[i] - 160;
                int num4 = bytes[i + 1] - 160;
                int num5 = (num3 * 100) + num4;
                if ((num5 > 0x640) && (num5 < 0x15d6))
                {
                    for (int j = 0x16; j >= 0; j--)
                    {
                        if (num5 >= numArray[j])
                        {
                            str2 = str2 + chArray[j];
                            break;
                        }
                    }
                }
                else
                {
                    int startIndex = (((num3 - 0x38) * 0x5e) + num4) - 1;
                    if ((startIndex >= 0) && (startIndex <= 0xbbf))
                    {
                        str2 = str2 + str.Substring(startIndex, 1);
                    }
                }
                i++;
            }
            return str2.ToLower();
        }

        /// <summary>
        /// 得到指定对象值的对应的字符串
        /// </summary>
        /// <param name="obj">对象值</param>
        /// <returns>对应的字符串</returns>
        public static string GetString(object obj)
        {
            object typeValue = GetTypeValue(typeof(string), obj);
            if (typeValue == null)
            {
                return "";
            }
            return (string)typeValue;
        }

        /// <summary>
        /// 转换指定的对象值为指定类型的值。
        /// </summary>
        /// <param name="targetType">要转换的类型</param>
        /// <param name="obj">对象值</param>
        /// <returns>转换后的值</returns>
        public static object GetTypeValue(Type targetType, object obj)
        {
            Type baseType = Reflector.GetNullable(targetType);
            bool isNullableType = baseType != null;

            if ((obj == null) || (obj == DBNull.Value))
            {
                if (targetType != null)
                {
                    if (isNullableType)
                    {
                        return null;
                    }

                    if (targetType == typeof(string))
                    {
                        return "";
                    }
                    if ((((targetType == typeof(int)) || (targetType == typeof(double))) ||
                         ((targetType == typeof(float)) || (targetType == typeof(decimal)))) ||
                        (targetType == typeof(long)))
                    {
                        int num = 0;
                        if (targetType == typeof(int))
                        {
                            return num;
                        }
                        return GetTypeValue(targetType, num);
                    }
                    if (targetType == typeof(DateTime))
                    {
                        return SysUtils.EmptyDateTime;
                    }
                }
                return null;
            }
            else if (targetType.IsEnum)
            {
                try
                {
                    return Enum.Parse(targetType, obj.ToString());
                }
                catch
                {
                    try
                    {
                        return Enum.ToObject(targetType, Convert.ToInt32(obj));
                    }
                    catch (Exception exception)
                    {
                        throw new System.ArithmeticException("枚举转换错误！");
                    }
                }
            }
            else
            {
                if (isNullableType)
                {
                    if (targetType == typeof(DateTime?))
                    {
                        if ((string)obj == "")
                            return null;
                    }
                    var nullableConvert = new NullableConverter(targetType);
                    return Convert.ChangeType(obj, nullableConvert.UnderlyingType);
                }
                else
                {
                    return Convert.ChangeType(obj, targetType);
                }
            }
        }


        /// <summary>
        /// 转换字符串为指定类型的值。
        /// </summary>
        /// <param name="targetType">要转换的类型</param>
        /// <param name="value">字符串</param>
        /// <returns>转换后的值</returns>
        public static object GetTypeValue(Type targetType, string value)
        {
            if (targetType == null)
                return null;

            if (Reflector.GetNullable(targetType) != null)
            {
                if (targetType == typeof(string))
                {
                    return value;
                }
                if (string.IsNullOrEmpty(value))
                {
                    return null;
                }
            }
            if (targetType.IsEnum)
            {
                try
                {
                    return Enum.Parse(targetType, value);
                }
                catch
                {
                    try
                    {
                        return Enum.ToObject(targetType, Convert.ToInt32(value));
                    }
                    catch (Exception exception)
                    {
                        throw new System.ArithmeticException("枚举转换异常！");
                    }
                }
            }
            if (null != value)
            {
                if (targetType == typeof(Guid))
                {
                    return new Guid(value);
                }
                if (targetType == typeof(string))
                {
                    return value;
                }
                return Convert.ChangeType(value, targetType);
            }
            return null;
        }

        /// <summary>
        /// 将数据库日期字符串转为时间类型(如果字符串格式不对，则返回emptyDateTime时间)
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="lenght">长度(4-年;6-年月;8-年月日;10-年月日时;12-年月日时分;14-年月日时分秒)</param>
        /// <returns>对应的时间</returns>
        public static DateTime ToDateTime(string str, int lenght)
        {
            if (str.Length != lenght)
            {
                return SysUtils.EmptyDateTime;
            }
            try
            {
                switch (lenght)
                {
                    case 4:
                        return new DateTime(int.Parse(str), 1, 1);

                    case 6:
                        return new DateTime(int.Parse(str.Substring(0, 4)), int.Parse(str.Substring(4, 2)), 1);

                    case 8:
                        return
                            new DateTime(int.Parse(str.Substring(0, 4)), int.Parse(str.Substring(4, 2)),
                                         int.Parse(str.Substring(6, 2)));

                    case 10:
                        return
                            new DateTime(int.Parse(str.Substring(0, 4)), int.Parse(str.Substring(4, 2)),
                                         int.Parse(str.Substring(6, 2)), int.Parse(str.Substring(8, 2)), 0, 0);

                    case 12:
                        return
                            new DateTime(int.Parse(str.Substring(0, 4)), int.Parse(str.Substring(4, 2)),
                                         int.Parse(str.Substring(6, 2)), int.Parse(str.Substring(8, 2)),
                                         int.Parse(str.Substring(10, 2)), 0);

                    case 14:
                        return
                            new DateTime(int.Parse(str.Substring(0, 4)), int.Parse(str.Substring(4, 2)),
                                         int.Parse(str.Substring(6, 2)), int.Parse(str.Substring(8, 2)),
                                         int.Parse(str.Substring(10, 2)), int.Parse(str.Substring(12, 2)));
                }
                return SysUtils.EmptyDateTime;
            }
            catch
            {
                return SysUtils.EmptyDateTime;
            }
        }

        /// <summary>
        /// 将字符串转为Double(如果字符不是正确的数字字符串,则返回0)
        /// </summary>
        /// <param name="str">表示数字的字符串</param>
        /// <returns>双精度数</returns>
        public static double ToDouble(string str)
        {
            if (string.IsNullOrEmpty(str.Trim()))
            {
                return 0.0;
            }
            try
            {
                double result = 0.0;
                if (double.TryParse(str, out result))
                {
                    return result;
                }
                return 0.0;
            }
            catch
            {
                return 0.0;
            }
        }

        /// <summary>
        /// 将字符串转换为decimal,如果为空或转换失败则返回0
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>转换的decimal</returns>
        public static decimal ToDecimal(string str)
        {
            if (string.IsNullOrEmpty(str.Trim()))
            {
                return 0;
            }
            try
            {
                decimal result = 0;
                if (decimal.TryParse(str, out result))
                {
                    return result;
                }
                return 0;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 将指定二进制数组加密(对称算法)
        /// </summary>
        /// <param name="normalBytes">未加密的二进制数组</param>
        /// <returns>加密后的内容</returns>
        public static byte[] ToEncryptData(byte[] normalBytes)
        {
            return ToEncryptData(normalBytes, encryptCode, encryptIV);
        }

        /// <summary>
        /// 将指定二进制数组加密(对称算法)
        /// </summary>
        /// <param name="normalBytes">未加密的二进制数组</param>
        /// <param name="encryptKeyCode">加密密码(0~12个字符)</param>
        /// <returns>加密后的内容</returns>
        public static byte[] ToEncryptData(byte[] normalBytes, string encryptKeyCode)
        {
            string str = "";
            if (encryptKeyCode == "")
            {
                str = encryptCode;
            }
            else if (encryptKeyCode.Length > 12)
            {
                str = encryptKeyCode.Substring(0, 12);
            }
            else
            {
                str = encryptKeyCode + new string('w', 12 - encryptKeyCode.Length);
            }
            return ToEncryptData(normalBytes, str, encryptIV);
        }

        /// <summary>
        /// 将指定二进制数组加密(对称算法)
        /// </summary>
        /// <param name="normalBytes">未加密的二进制数组</param>
        /// <param name="encryptKeyCode">加密密钥(12位密钥)</param>
        /// <param name="encryptIV">加密向量(16个元素)</param>
        /// <returns>加密后的内容</returns>
        public static byte[] ToEncryptData(byte[] normalBytes, string encryptKeyCode, byte[] encryptIV)
        {
            var stream = new MemoryStream();
            byte[] bytes = Encoding.Default.GetBytes(encryptKeyCode);
            byte[] rgbIV = encryptIV;
            ICryptoTransform transform = Rijndael.Create().CreateEncryptor(bytes, rgbIV);
            var stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Write);
            stream2.Write(normalBytes, 0, normalBytes.Length);
            stream2.FlushFinalBlock();
            byte[] buffer3 = stream.ToArray();
            stream2.Close();
            stream.Close();
            return buffer3;
        }

        /// <summary>
        /// 将指定的二进制数组转为对应的压缩数据。
        /// </summary>
        /// <param name="normalBytes">未压缩的数据</param>
        /// <returns>压缩后的数据</returns>
        public static byte[] ToGZipData(byte[] normalBytes)
        {
            return SevenZip.SevenZipCompressor.CompressBytes(normalBytes);
        }

        /// <summary>
        /// 将正常数据包转为通讯数据包(先压缩再加密)
        /// </summary>
        /// <param name="normalData">正常数据包</param>
        /// <returns>通讯数据包</returns>
        public static byte[] ToGZipEncryptData(byte[] normalData)
        {
            try
            {
                return ToEncryptData(ToGZipData(normalData));
            }
            catch
            {
                return normalData;
            }
        }

        /// <summary>
        /// 将字符串转为整型(如果字符不是正确的数字字符串,则返回0)
        /// </summary>
        /// <param name="str">表示数字的字符串</param>
        /// <returns>整数</returns>
        public static int ToInt32(string str)
        {
            if (str.Trim() == "")
            {
                return 0;
            }
            try
            {
                int result = 0;
                if (int.TryParse(str, out result))
                {
                    return result;
                }
                return 0;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 将指定二进制数组解密(对称算法)
        /// </summary>
        /// <param name="encryptBytes">加密的二进制数组</param>
        /// <returns>解密后的内容</returns>
        public static byte[] ToNormalData(byte[] encryptBytes)
        {
            return ToNormalData(encryptBytes, encryptCode, encryptIV);
        }

        /// <summary>
        /// 将指定二进制数组解密(对称算法)
        /// </summary>
        /// <param name="encryptBytes">加密的二进制数组</param>
        /// <param name="encryptKeyCode">加密密码(0~12个字符)</param>
        /// <returns>解密后的内容</returns>
        public static byte[] ToNormalData(byte[] encryptBytes, string encryptKeyCode)
        {
            string str = "";
            if (encryptKeyCode == "")
            {
                str = encryptCode;
            }
            else if (encryptKeyCode.Length > 12)
            {
                str = encryptKeyCode.Substring(0, 12);
            }
            else
            {
                str = encryptKeyCode + new string('z', 12 - encryptKeyCode.Length);
            }
            return ToNormalData(encryptBytes, str, encryptIV);
        }

        /// <summary>
        /// 将指定二进制数组解密(对称算法)
        /// </summary>
        /// <param name="encryptBytes">加密的二进制数组</param>
        /// <param name="encryptKeyCode">加密密钥(12位密钥)</param>
        /// <param name="encryptIV">加密向量(16个元素)</param>
        /// <returns>解密后的内容</returns>
        public static byte[] ToNormalData(byte[] encryptBytes, string encryptKeyCode, byte[] encryptIV)
        {
            var stream = new MemoryStream(encryptBytes);
            byte[] bytes = Encoding.Default.GetBytes(encryptKeyCode);
            byte[] rgbIV = encryptIV;
            ICryptoTransform transform = Rijndael.Create().CreateDecryptor(bytes, rgbIV);
            var stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Read);
            var buffer = new byte[0x400];
            var stream3 = new MemoryStream();
            for (int i = stream2.Read(buffer, 0, buffer.Length); i > 0; i = stream2.Read(buffer, 0, buffer.Length))
            {
                stream3.Write(buffer, 0, i);
            }
            buffer = stream3.ToArray();
            stream2.Close();
            stream.Close();
            stream3.Close();
            return buffer;
        }

        /// <summary>
        /// 将通讯数据包转为正常数据包(先解密再解压)
        /// </summary>
        /// <param name="zipEncryptData">通讯数据包</param>
        /// <returns>正常数据包</returns>
        public static byte[] ToNormalUnGZip(byte[] zipEncryptData)
        {
            try
            {
                return ToUnGZipData(ToNormalData(zipEncryptData));
            }
            catch
            {
                return zipEncryptData;
            }
        }

        /// <summary>
        /// 将指定的压缩的二进制数组解压缩
        /// </summary>
        /// <param name="zipBytes">压缩数据</param>
        /// <returns>解压的数据</returns>
        public static byte[] ToUnGZipData(byte[] zipBytes)
        {
            return SevenZip.SevenZipExtractor.ExtractBytes(zipBytes);
        }

        /// <summary>
        /// 图像到二进制转换
        /// </summary>
        /// <returns></returns>
        public static byte[] ImageToBytes(Image image)
        {
            if (image == null)
            {
                return new byte[0];
            }

            var ms = new MemoryStream();

            image.Save(ms, image.RawFormat);
            return ms.ToArray();
        }

        /// <summary>
        /// 把二进制数据转换为图像
        /// </summary>
        /// <param name="imageBytes"></param>
        /// <returns></returns>
        public static Image BytesToImage(byte[] imageBytes)
        {
            if (imageBytes == null || imageBytes.Length == 0)
            {
                return null;
            }
            var ms = new MemoryStream(imageBytes);
            return Image.FromStream(ms);
        }

        /// <summary>
        /// 将字符串转化为字节流
        /// </summary>
        /// <param name="strSource">字串</param>
        /// <returns>字节流</returns>
        public static byte[] StringToBytes(string strSource)
        {
            return System.Text.Encoding.UTF8.GetBytes(strSource);
        }

        /// <summary>
        /// 将字节流转化为字符串
        /// </summary>
        /// <param name="bytData">字节流</param>
        /// <returns>字串</returns>
        public static string BytesToString(byte[] bytData)
        {
            return Encoding.UTF8.GetString(bytData);
        }

        /// <summary>
        /// 字符串转换为布尔类型
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static bool ToBoolean(string strValue)
        {
            int i = ToInt32(strValue);
            return i != 0;
        }

        /// <summary>
        /// 布尔类型转换为字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string BooleanToString(bool value)
        {
            return value ? "1" : "0";
        }

        /// <summary>
        /// 将对象序列化为内存流,返回的流没有关闭
        /// 使用二进制序列化
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static MemoryStream Serialize(object data)
        {
            var ms = new MemoryStream();
            var binarySerial = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            try
            {
                binarySerial.Serialize(ms, data);
                ms.Flush();
                ms.Position = 0;
                //byte[] dat = ToGZipData(ms.ToArray());
                //ms.SetLength(0);
                //ms.Capacity = 0;
                //ms.Write(dat, 0, dat.Length);
                return ms;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                //ms.Close();
            }
        }

        /// <summary>
        /// 反序列化流为对象
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static object Deserialize(MemoryStream stream)
        {
            // 解压缩
            //byte[] data = new byte[stream.Length];
            //stream.Read(data, 0, (int)stream.Length);
            //byte[] newData = ToUnGZipData(stream.ToArray());
            //stream.SetLength(0);
            //stream.Capacity = 0;
            //stream.Position = 0;
            //stream.Write(newData, 0, newData.Length);
            //stream.Position = 0;

            var binarySerial = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            try
            {
                return binarySerial.Deserialize(stream);
            }
            catch (Exception ex)
            {
                return null;
            }
        }    

        #region  金额大写转换

        /// <summary>
        /// 大写数据
        /// </summary>
        private const string DXSZ = "零壹贰叁肆伍陆柒捌玖";
        /// <summary>
        /// 大写单位
        /// </summary>
        private const string DXDW = "毫厘分角元拾佰仟萬拾佰仟亿拾佰仟萬兆拾佰仟萬亿京拾佰仟萬亿兆垓";
        /// <summary>
        /// 
        /// </summary>
        private const string SCDW = "元拾佰仟萬亿京兆垓";

        /// <summary>
        /// 转换整数为大写金额
        /// 最高精度为垓，保留小数点后4位，实际精度为亿兆已经足够了，理论上精度无限制，如下所示：
        /// 序号:...30.29.28.27.26.25.24  23.22.21.20.19.18  17.16.15.14.13  12.11.10.9   8 7.6.5.4  . 3.2.1.0
        /// 单位:...垓兆亿萬仟佰拾        京亿萬仟佰拾       兆萬仟佰拾      亿仟佰拾     萬仟佰拾元 . 角分厘毫
        /// 数值:...1000000               000000             00000           0000         00000      . 0000
        /// 下面列出网上搜索到的数词单位：
        /// 元、十、百、千、万、亿、兆、京、垓、秭、穰、沟、涧、正、载、极
        /// </summary>
        /// <param name="capValue">整数值</param>
        /// <returns>返回大写金额</returns>
        private static string ConvertIntToUppercaseAmount(string capValue)
        {
            string currCap = "";    //当前金额
            string capResult = "";  //结果金额
            string currentUnit = "";//当前单位
            string resultUnit = ""; //结果单位            
            int prevChar = -1;      //上一位的值
            int currChar = 0;       //当前位的值
            int posIndex = 4;       //位置索引，从"元"开始

            if (Convert.ToDouble(capValue) == 0) return "";
            for (int i = capValue.Length - 1; i >= 0; i--)
            {
                currChar = Convert.ToInt16(capValue.Substring(i, 1));
                if (posIndex > 30)
                {
                    //已超出最大精度"垓"。注：可以将30改成22，使之精确到兆亿就足够了
                    break;
                }
                else if (currChar != 0)
                {
                    //当前位为非零值，则直接转换成大写金额
                    currCap = DXSZ.Substring(currChar, 1) + DXDW.Substring(posIndex, 1);
                }
                else
                {
                    //防止转换后出现多余的零,例如：3000020
                    switch (posIndex)
                    {
                        case 4: currCap = "元"; break;
                        case 8: currCap = "萬"; break;
                        case 12: currCap = "亿"; break;
                        case 17: currCap = "兆"; break;
                        case 23: currCap = "京"; break;
                        case 30: currCap = "垓"; break;
                        default: break;
                    }
                    if (prevChar != 0)
                    {
                        if (currCap != "")
                        {
                            if (currCap != "元") currCap += "零";
                        }
                        else
                        {
                            currCap = "零";
                        }
                    }
                }
                //对结果进行容错处理               
                if (capResult.Length > 0)
                {
                    resultUnit = capResult.Substring(0, 1);
                    currentUnit = DXDW.Substring(posIndex, 1);
                    if (SCDW.IndexOf(resultUnit) > 0)
                    {
                        if (SCDW.IndexOf(currentUnit) > SCDW.IndexOf(resultUnit))
                        {
                            capResult = capResult.Substring(1);
                        }
                    }
                }
                capResult = currCap + capResult;
                prevChar = currChar;
                posIndex += 1;
                currCap = "";
            }
            return capResult;
        }

        /// <summary>
        /// 转换小数为大写金额
        /// </summary>
        /// <param name="capValue">小数值</param>
        /// <param name="addZero">是否增加零位</param>
        /// <returns>返回大写金额</returns>
        private static string ConvertDecToUppercaseAmount(string capValue, bool addZero)
        {
            string currCap = "";
            string capResult = "";
            int prevChar = addZero ? -1 : 0;
            int currChar = 0;
            int posIndex = 3;

            if (Convert.ToInt16(capValue) == 0) return "";
            for (int i = 0; i < capValue.Length; i++)
            {
                currChar = Convert.ToInt16(capValue.Substring(i, 1));
                if (currChar != 0)
                {
                    currCap = DXSZ.Substring(currChar, 1) + DXDW.Substring(posIndex, 1);
                }
                else
                {
                    if (Convert.ToInt16(capValue.Substring(i)) == 0)
                    {
                        break;
                    }
                    else if (prevChar != 0)
                    {
                        currCap = "零";
                    }
                }
                capResult += currCap;
                prevChar = currChar;
                posIndex -= 1;
                currCap = "";
            }
            return capResult;
        }



        /// <summary>
        /// 人民币大写金额
        /// </summary>
        /// <param name="value">人民币数字金额值</param>
        /// <returns>返回人民币大写/小写金额</returns>
        public static string RMBAmount(decimal value)
        {
            string capResult = "";
            string capValue = String.Format("{0:f4}", value);
            int dotPos = capValue.IndexOf(".");
            bool addInt = (dotPos == 0);
            bool addMinus = (capValue.Substring(0, 1) == "-");
            int beginPos = addMinus ? 1 : 0;
            string capInt = capValue.Substring(beginPos, addMinus ? dotPos - 1 : dotPos);
            string capDec = capValue.Substring(dotPos + 1);
            if (capDec.Length > 0 && Convert.ToInt32(capDec) == 0)
            {
                addInt = true;
            }

            if (dotPos > 0)
            {
                capResult = ConvertIntToUppercaseAmount(capInt) +
                            ConvertDecToUppercaseAmount(capDec, Convert.ToDouble(capInt) != 0);
            }
            else
            {
                capResult = ConvertIntToUppercaseAmount(capDec);
            }
            if (addMinus) capResult = "负" + capResult;
            if (addInt) capResult += "整";
            return capResult;
        }


        #endregion
    }
}