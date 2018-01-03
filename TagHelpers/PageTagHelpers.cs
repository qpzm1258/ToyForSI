using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.IO;
using ZXing.QrCode;
using System.Text;

namespace ToyForSI.TagHelpers
{
    [HtmlTargetElement("pager")]
    public class PagerTagHelper : TagHelper
    {
        public MoPagerOption PagerOption { get; set; }

        
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

            output.TagName = "div";

            if (PagerOption.PageSize <= 0) { PagerOption.PageSize = 15; }
            if (PagerOption.CurrentPage <= 0) { PagerOption.CurrentPage = 1; }
            if (PagerOption.Total <= 0) { return; }

            //总页数
            var totalPage = PagerOption.Total / PagerOption.PageSize + (PagerOption.Total % PagerOption.PageSize > 0 ? 1 : 0);
            if (totalPage <= 0) { return; }
            //当前路由地址
            if (string.IsNullOrEmpty(PagerOption.RouteUrl))
            {

               //PagerOption.RouteUrl = helper.ViewContext.HttpContext.Request.RawUrl;
                if (!string.IsNullOrEmpty(PagerOption.RouteUrl))
                {

                    var lastIndex = PagerOption.RouteUrl.LastIndexOf("/");
                    PagerOption.RouteUrl = PagerOption.RouteUrl.Substring(0, lastIndex);
                }
            }
            PagerOption.RouteUrl = PagerOption.RouteUrl.TrimEnd('/');

            //构造分页样式
            var sbPage = new StringBuilder(string.Empty);
            switch (PagerOption.StyleNum)
            {
                case 2:
                    {
                        break;
                    }
                default:
                    {
                        #region 默认样式

                        sbPage.Append("<nav>");
                        sbPage.Append("  <ul class=\"pagination\">");
                        if(PagerOption.CurrentPage-2>1&&totalPage>7)  
                        {
                             sbPage.AppendFormat("       <li><a href=\"{0}?page=1\" aria-label=\"Previous\"><span aria-hidden=\"true\" class=\"glyphicon glyphicon-step-backward\"></span></a></li>",
                                                PagerOption.RouteUrl);
                        }
                        sbPage.AppendFormat("       <li><a href=\"{0}?page={1}\" aria-label=\"Previous\"><span aria-hidden=\"true\" class=\"glyphicon glyphicon-triangle-left\"></span></a></li>",
                                                PagerOption.RouteUrl,
                                                PagerOption.CurrentPage - 1 <= 0 ? 1 : PagerOption.CurrentPage - 1);
                        if(PagerOption.CurrentPage-2>1)  
                        {
                            int startidx= 3-totalPage+PagerOption.CurrentPage;
                            startidx=startidx>0?startidx:0;
                            startidx=PagerOption.CurrentPage-2-startidx;
                            startidx=startidx<=1?1:startidx;
                            if(totalPage<=7)
                            {
                                startidx=1;
                            }
                            for (int i = startidx; i <= PagerOption.CurrentPage; i++)
                            {

                                sbPage.AppendFormat("       <li {1}><a href=\"{2}?page={0}\">{0}</a></li>",
                                    i,
                                    i == PagerOption.CurrentPage ? "class=\"active\"" : "",
                                    PagerOption.RouteUrl);

                            }
                        } 
                        else
                        {
                            for (int i = 1; i <= PagerOption.CurrentPage; i++)
                            {

                                sbPage.AppendFormat("       <li {1}><a href=\"{2}?page={0}\">{0}</a></li>",
                                    i,
                                    i == PagerOption.CurrentPage ? "class=\"active\"" : "",
                                    PagerOption.RouteUrl);

                            }
                        }
                        
                        if(totalPage-PagerOption.CurrentPage>2)
                        {
                            int endidx=totalPage>7?Math.Min(totalPage,PagerOption.CurrentPage+2+(4-PagerOption.CurrentPage>0?4-PagerOption.CurrentPage:0)):7;
                             for (int i = PagerOption.CurrentPage+1; i <= endidx; i++)
                            {

                                sbPage.AppendFormat("       <li {1}><a href=\"{2}?page={0}\">{0}</a></li>",
                                    i,
                                    i == PagerOption.CurrentPage ? "class=\"active\"" : "",
                                    PagerOption.RouteUrl);

                            }
                        }
                        else
                        {
                              for (int i = PagerOption.CurrentPage+1; i <= totalPage; i++)
                            {

                                sbPage.AppendFormat("       <li {1}><a href=\"{2}?page={0}\">{0}</a></li>",
                                    i,
                                    i == PagerOption.CurrentPage ? "class=\"active\"" : "",
                                    PagerOption.RouteUrl);

                            }
                        }

                        // for (int i = 1; i <= totalPage; i++)
                        // {

                        //     sbPage.AppendFormat("       <li {1}><a href=\"{2}?page={0}\">{0}</a></li>",
                        //         i,
                        //         i == PagerOption.CurrentPage ? "class=\"active\"" : "",
                        //         PagerOption.RouteUrl);

                        // }

                        sbPage.Append("       <li>");
                        sbPage.AppendFormat("         <a href=\"{0}?page={1}\" aria-label=\"Next\">",
                                            PagerOption.RouteUrl,
                                            PagerOption.CurrentPage + 1 > totalPage ? PagerOption.CurrentPage : PagerOption.CurrentPage + 1);
                        sbPage.Append("               <span aria-hidden=\"true\" class=\"glyphicon glyphicon-triangle-right\"></span>");
                        sbPage.Append("         </a>");
                        sbPage.Append("       </li>");
                        if(totalPage-PagerOption.CurrentPage>2&&totalPage>7)
                        {
                             sbPage.AppendFormat("       <li><a href=\"{0}?page={1}\" aria-label=\"Previous\"><span aria-hidden=\"true\" class=\"glyphicon glyphicon-step-forward\"></span></a></li>",
                                                PagerOption.RouteUrl,
                                                totalPage);
                        }
                        sbPage.Append("   </ul>");
                        sbPage.Append("</nav>");
                        #endregion
                    }
                    break;
            }

            output.Content.SetHtmlContent(sbPage.ToString());
            //output.TagMode = TagMode.SelfClosing;
            //return base.ProcessAsync(context, output);
        }


    }
    /// <summary>
    /// 分页option属性
    /// </summary>
    public class MoPagerOption
    {
        /// <summary>
        /// 当前页  必传
        /// </summary>
        public int CurrentPage { get; set; }
        /// <summary>
        /// 总条数  必传
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 分页记录数（每页条数 默认每页15条）
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 路由地址(格式如：/Controller/Action) 默认自动获取
        /// </summary>
        public string RouteUrl { get; set; }

        /// <summary>
        /// 样式 默认 bootstrap样式 1
        /// </summary>
        public int StyleNum { get; set; }
    }
}