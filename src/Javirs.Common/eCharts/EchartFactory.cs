using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Javirs.Common.eCharts
{
    /// <summary>
    /// 图表工厂
    /// </summary>
    public class EchartFactory
    {
        /// <summary>
        /// 新建折线图
        /// </summary>
        /// <param name="text">折线图标题</param>
        /// <param name="legend">线条分类</param>
        /// <param name="axis">横轴</param>
        /// <param name="data">显示数据</param>
        /// <returns></returns>
        public static eChart BuildLineChart(string text, string[] legend, string[] axis, int[][] data)
        {
            var seriesArray = new Series[legend.Length];
            for (int i = 0; i < legend.Length; i++)
            {
                var series = new Series
                {
                    name = legend[i],
                    type = "line",
                    data = data[i].Cast<object>().ToArray()
                };
                seriesArray[i] = series;
            }
            var chart = new eChart
            {
                title = new Title { text = text },
                legend = new Legend
                {
                    data = legend
                },
                series = seriesArray,
                xAxis = new Axis
                {
                    data = axis
                },
                yAxis = new Axis
                {
                    type = "value"
                },
                tooltip = new Tooltip
                {
                    trigger = "axis"
                }
            };
            return chart;
        }
        /// <summary>
        /// 建立一个饼状图
        /// </summary>
        /// <param name="text"></param>
        /// <param name="legend_data"></param>
        /// <param name="axis"></param>
        /// <param name="data"></param>
        /// <param name="radius">饼图的大小</param>
        /// <param name="xPos">x坐标，百分比，默认值50（%）</param>
        /// <param name="yPos">y坐标，百分比，默认值50（%）</param>
        /// <returns></returns>
        public static eChart BuildPieChart(string text, string[] legend_data, object[] data, int radius = 50, int xPos = 50, int yPos = 50)
        {
            radius = radius > 100 || radius <= 0 ? 50 : radius;
            xPos = xPos > 100 || xPos <= 0 ? 50 : xPos;
            yPos = yPos > 100 || yPos <= 0 ? 50 : yPos;
            var seriesArray = new Series[1];
            for (int i = 0; i < seriesArray.Length; i++)
            {
                var series = new Series
                {
                    name = text,
                    type = "pie",
                    data = data,
                    radius = radius + "%",
                    center = new string[] { xPos + "%", yPos + "%" },
                };
                seriesArray[i] = series;
            }
            var chart = new eChart
            {
                title = new Title { text = text, x = "center" },
                legend = new Legend
                {
                    data = legend_data,
                    orient = "vertical",
                    left = "left"
                },
                tooltip = new Tooltip
                {
                    trigger = "item"
                },
                series = seriesArray,

            };
            return chart;
        }
    }
}
