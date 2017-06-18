using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace HtmlExtension
{
    /// <summary>
    /// Class for getting info form html pages
    /// </summary>
    public static class HtmlHelper
    {
        /// <summary>
        /// Get string response from HTTP GET request
        /// </summary>
        /// <param name="url">url of website</param>
        /// <returns></returns>
        public static string GetRequest(string url)
        {
            string page = string.Empty;

            try
            {
                HttpWebRequest http = WebRequest.CreateHttp(url);
                WebResponse wr = http.GetResponse();

                // get full page
                using (var rs = wr.GetResponseStream())
                using (var sr = new StreamReader(rs))
                    page = sr.ReadToEnd();

            }
            catch (Exception) { }

            return page;
        }


        /// <summary>
        /// Get a position of closed tag
        /// </summary>
        /// <param name="text">info block of searching</param>
        /// <param name="OpenTagPosition">type of tag</param>
        /// <returns></returns>
        public static int GetClosedTagPosition(string text, int OpenTagPosition)
        {
            int res = -1;

            try
            {
                // start position of tag
                int startTag = text.IndexOf('<', OpenTagPosition) + 1;

                if (startTag != -1)
                {
                    // end position of tag
                    int endTag = text.IndexOfAny(new char[] { ' ', '>' }, startTag);

                    if (endTag != -1)
                    {
                        // tag value
                        string tag = text.Substring(startTag, endTag - startTag);

                        // create a stack and add the tag to it
                        Stack<string> stack = new Stack<string>();
                        stack.Push(tag);

                        while (true)
                        {
                            // find the next tag
                            int nextTag = text.IndexOf(tag, endTag);

                            if (nextTag == -1)
                                // end of text
                                break;

                            if (text[nextTag - 1] != '/')
                                // another opened tag
                                stack.Push(tag);
                            else
                                // closed tag
                                stack.Pop();

                            if (stack.Count == 0)
                            {
                                // found our closed tag
                                res = nextTag + tag.Length + 1;
                                break;
                            }

                            // move cursor position
                            endTag = nextTag + tag.Length;
                        }
                    }
                }
            }
            catch (Exception) { }

            return res;
        }

        /// <summary>
        /// Get a value which situated between opened and closed tag
        /// </summary>
        /// <param name="text">info block of searching</param>
        /// <param name="tag">type of tag</param>
        /// <returns></returns>
        public static string GetValueOfSimpleTag(string text, string tag)
        {
            string res = string.Empty;

            try
            {
                // opened tag with brackets
                string openedTag = $"<{tag}>";

                // closed tag with brackets
                string closedTag = $"</{tag}>";

                // scope of value
                int startValue = text.IndexOf(openedTag) + openedTag.Length;
                int endValue = text.IndexOf(closedTag, startValue);

                res = text.Substring(startValue, endValue - startValue);
            }
            catch (Exception ex)
            {
                throw new Exception("TagValue has not found", ex);
            }

            return res;
        }
    }
}
