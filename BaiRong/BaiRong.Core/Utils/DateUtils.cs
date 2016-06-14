using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Specialized;

using System.Globalization;
using BaiRong.Model;

namespace BaiRong.Core
{
	public class DateUtils
	{
        private DateUtils()
		{
		}

        public const string FormatStringDateTime = "yyyy-MM-dd HH:mm";
        public const string FormatStringDateOnly = "yyyy-MM-dd";

        public static string GetRelatedDateTimeString(DateTime datetime)
        {
            string retval = string.Empty;
            TimeSpan interval = DateTime.Now - datetime;
            if (interval.Days > 0)
            {
                if (interval.Days >= 7 && interval.Days < 35)
                {
                    retval = string.Format("{0}��", interval.Days / 7);
                }
                else
                {
                    retval = string.Format("{0}��", interval.Days);
                }
            }
            else if (interval.Hours > 0)
            {
                retval = string.Format("{0}Сʱ", interval.Hours);
            }
            else if (interval.Minutes > 0)
            {
                retval = string.Format("{0}����", interval.Minutes);
            }
            else if (interval.Seconds > 0)
            {
                retval = string.Format("{0}��", interval.Seconds);
            }
            else if (interval.Milliseconds > 0)
            {
                retval = string.Format("{0}����", interval.Milliseconds);
            }
            else
            {
                retval = GetDateAndTimeString(datetime);
            }
            return retval;
        }

        public static string GetRelatedDateTimeString(DateTime datetime, string postfix)
        {
            return string.Format("{0}{1}", GetRelatedDateTimeString(datetime), postfix);
        }

        public static string GetDateAndTimeString(DateTime datetime, EDateFormatType dateFormat, ETimeFormatType timeFormat)
        {
            return string.Format("{0} {1}", GetDateString(datetime, dateFormat), GetTimeString(datetime, timeFormat));
        }

        public static string GetDateAndTimeString(DateTime datetime)
        {
            if (datetime == DateUtils.SqlMinValue) return string.Empty;
            return GetDateAndTimeString(datetime, EDateFormatType.Day, ETimeFormatType.ShortTime);
        }

        public static string GetDateString(DateTime datetime)
        {
            if (datetime == DateUtils.SqlMinValue) return string.Empty;
            return GetDateString(datetime, EDateFormatType.Day);
        }

        public static string GetDateString(DateTime datetime, EDateFormatType dateFormat)
        {
            string format = string.Empty;
            switch (dateFormat)
            {
                case EDateFormatType.Year:
                    format = "yyyy��MM��";
                    break;

                case EDateFormatType.Month:
                    format = "MM��dd��";
                    break;

                case EDateFormatType.Day:
                    format = "yyyy-MM-dd";
                    break;

                case EDateFormatType.Chinese:
                    format = "yyyy��M��d��";
                    break;

                default:
                    break;
            }
            return datetime.ToString(format);
        }

		public static string GetTimeString(DateTime datetime)
		{
			return GetTimeString(datetime, ETimeFormatType.ShortTime);
		}

        public static string GetTimeString(DateTime datetime, ETimeFormatType timeFormat)
        {
            string retval = string.Empty;
            switch (timeFormat)
            {
                case ETimeFormatType.LongTime:
                    retval = datetime.ToLongTimeString();
                    break;

                case ETimeFormatType.ShortTime:
                    retval = datetime.ToShortTimeString();
                    break;

                default:
                    break;
            }
            return retval;
        }

		public static int GetSeconds(string intWithUnitString)
		{
			int seconds = 0;
			if (!string.IsNullOrEmpty(intWithUnitString))
			{
				intWithUnitString = intWithUnitString.Trim().ToLower();
				if (intWithUnitString.EndsWith("h"))
				{
					seconds = 60 * 60 * TranslateUtils.ToInt(intWithUnitString.TrimEnd('h'));
				}
				else if (intWithUnitString.EndsWith("m"))
				{
					seconds = 60 * TranslateUtils.ToInt(intWithUnitString.TrimEnd('m'));
				}
				else if (intWithUnitString.EndsWith("s"))
				{
					seconds = TranslateUtils.ToInt(intWithUnitString.TrimEnd('s'));
				}
				else
				{
					seconds = TranslateUtils.ToInt(intWithUnitString);
				}
			}
			return seconds;
		}

        public static bool IsSince(string val)
        {
            if (!string.IsNullOrEmpty(val))
            {
                val = val.Trim().ToLower();
                if (val.EndsWith("y") || val.EndsWith("m") || val.EndsWith("d") || val.EndsWith("h"))
                {
                    return true;
                }
            }
            return false;
        }

        public static int GetSinceHours(string intWithUnitString)
        {
            int hours = 0;
            if (!string.IsNullOrEmpty(intWithUnitString))
            {
                intWithUnitString = intWithUnitString.Trim().ToLower();
                if (intWithUnitString.EndsWith("y"))
                {
                    hours = 8760 * TranslateUtils.ToInt(intWithUnitString.TrimEnd('y'));
                }
                else if (intWithUnitString.EndsWith("m"))
                {
                    hours = 720 * TranslateUtils.ToInt(intWithUnitString.TrimEnd('m'));
                }
                else if (intWithUnitString.EndsWith("d"))
                {
                    hours = 24 * TranslateUtils.ToInt(intWithUnitString.TrimEnd('d'));
                }
                else if (intWithUnitString.EndsWith("h"))
                {
                    hours = TranslateUtils.ToInt(intWithUnitString.TrimEnd('h'));
                }
                else
                {
                    hours = TranslateUtils.ToInt(intWithUnitString);
                }
            }
            return hours;
        }

        public static bool IsTheSameDay(DateTime d1, DateTime d2)
        {
            if (d1.Year == d2.Year && d1.Month == d2.Month && d1.Day == d2.Day)
            {
                return true;
            }
            return false;
        }

        public static string Format(DateTime datetime, string formatString)
        {
            string retval = string.Empty;
            if (!string.IsNullOrEmpty(formatString))
            {
                if (formatString.IndexOf("{0:") != -1)
                {
                    retval = string.Format(DateTimeFormatInfo.InvariantInfo, formatString, datetime);
                }
                else
                {
                    retval = datetime.ToString(formatString, DateTimeFormatInfo.InvariantInfo);
                }
            }
            else
            {
                retval = DateUtils.GetDateString(datetime);
            }
            return retval;
        }

		public static DateTime SqlMinValue
		{
			get
			{
				return new DateTime(1754, 1, 1, 0, 0, 0, 0);
			}
		}

        //Task used
        public static int GetDayOfWeek(DateTime dateTime)
        {
            switch (dateTime.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return 1;

                case DayOfWeek.Tuesday:
                    return 2;

                case DayOfWeek.Wednesday:
                    return 3;

                case DayOfWeek.Thursday:
                    return 4;

                case DayOfWeek.Friday:
                    return 5;

                case DayOfWeek.Saturday:
                    return 6;

                default:
                    return 7;
            }
        }

        public static string ParseThisMoment(DateTime dateTime)
        {
            if (dateTime != DateUtils.SqlMinValue)
            {
                return ParseThisMoment(dateTime, DateTime.Now);
            }
            return string.Empty;
        }

        /// <summary>
        /// ������ʱ�������ڵ�ʱ���ý��죬���죬ǰ���ʾ�����ʱ�䣬������
        /// </summary>
        /// <param name="date">���Ƚϵ�ʱ��</param>
        /// <param name="currentDateTime">Ŀ��ʱ��</param>
        /// <returns></returns>
        public static string ParseThisMoment(DateTime dateTime, DateTime currentDateTime)
        {
            string result = "";
            if (currentDateTime.Year == dateTime.Year && currentDateTime.Month == dateTime.Month)//���date�͵�ǰʱ����ݻ����·ݲ�һ�£���ֱ�ӷ���"yyyy-MM-dd HH:mm"��ʽ����
            {
                if (DateDiff("hour", dateTime, currentDateTime) <= 10)//���date�͵�ǰʱ������10Сʱ��(������3Сʱ)
                {
                    if (DateDiff("hour", dateTime, currentDateTime) > 0)
                        return DateDiff("hour", dateTime, currentDateTime) + "Сʱǰ";

                    if (DateDiff("minute", dateTime, currentDateTime) > 0)
                        return DateDiff("minute", dateTime, currentDateTime) + "����ǰ";

                    if (DateDiff("second", dateTime, currentDateTime) >= 0)
                        return DateDiff("second", dateTime, currentDateTime) + "��ǰ";
                    else
                        return "�ղ�";//Ϊ�˽��ʱ�侫�Ȳ������·���ʱ������ļ���
                }
                else
                {
                    switch (currentDateTime.Day - dateTime.Day)
                    {
                        case 0:
                            result = "���� " + dateTime.ToString("HH") + ":" + dateTime.ToString("mm");
                            break;
                        case 1:
                            result = "���� " + dateTime.ToString("HH") + ":" + dateTime.ToString("mm");
                            break;
                        case 2:
                            result = "ǰ�� " + dateTime.ToString("HH") + ":" + dateTime.ToString("mm");
                            break;
                        default:
                            result = dateTime.ToString("yyyy-MM-dd HH:mm");
                            break;
                    }
                }
            }
            else
                result = dateTime.ToString("yyyy-MM-dd HH:mm");
            return result;
        }

        /// <summary>
        /// ����ʱ��Ĳ�ֵ������Ϊ�룬Сʱ���죬����
        /// </summary>
        /// <param name="Interval">��Ҫ�õ���ʱ�ʽ</param>
        /// <param name="StartDate">��ʼʱ��</param>
        /// <param name="EndDate">����ʱ��</param>
        /// <returns></returns>
        public static long DateDiff(string interval, DateTime startDate, DateTime endDate)
        {
            long lngDateDiffValue = 0;
            System.TimeSpan TS = new System.TimeSpan(endDate.Ticks - startDate.Ticks);
            switch (interval)
            {
                case "second":
                    lngDateDiffValue = (long)TS.TotalSeconds;
                    break;
                case "minute":
                    lngDateDiffValue = (long)TS.TotalMinutes;
                    break;
                case "hour":
                    lngDateDiffValue = (long)TS.TotalHours;
                    break;
                case "day":
                    lngDateDiffValue = (long)TS.Days;
                    break;
                case "week":
                    lngDateDiffValue = (long)(TS.Days / 7);
                    break;
                case "month":
                    lngDateDiffValue = (long)(TS.Days / 30);
                    break;
                case "quarter":
                    lngDateDiffValue = (long)((TS.Days / 30) / 3);
                    break;
                case "year":
                    lngDateDiffValue = (long)(TS.Days / 365);
                    break;
            }
            return (lngDateDiffValue);
        }
	}
}
