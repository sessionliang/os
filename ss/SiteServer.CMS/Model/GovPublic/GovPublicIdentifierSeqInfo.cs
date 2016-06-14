using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;

namespace SiteServer.CMS.Model
{
	public class GovPublicIdentifierSeqInfo
	{
        private int seqID;
        private int publishmentSystemID;
        private int nodeID;
        private int departmentID;
        private int addYear;
        private int sequence;

		public GovPublicIdentifierSeqInfo()
		{
            this.seqID = 0;
            this.publishmentSystemID = 0;
            this.nodeID = 0;
            this.departmentID = 0;
            this.addYear = 0;
            this.sequence = 0;
		}

        public GovPublicIdentifierSeqInfo(int seqID, int publishmentSystemID, int nodeID, int departmentID, int addYear, int sequence)
		{
            this.seqID = seqID;
            this.publishmentSystemID = publishmentSystemID;
            this.nodeID = nodeID;
            this.departmentID = departmentID;
            this.addYear = addYear;
            this.sequence = sequence;
		}

        public int SeqID
		{
            get { return seqID; }
            set { seqID = value; }
		}

        public int PublishmentSystemID
        {
            get { return publishmentSystemID; }
            set { publishmentSystemID = value; }
        }

        public int NodeID
		{
            get { return nodeID; }
            set { nodeID = value; }
		}

        public int DepartmentID
		{
            get { return departmentID; }
            set { departmentID = value; }
		}

        public int AddYear
		{
            get { return addYear; }
            set { addYear = value; }
		}

        public int Sequence
        {
            get { return sequence; }
            set { sequence = value; }
        }
	}
}
