﻿// COPYRIGHT 2011, 2012, 2013 by the Open Rails project.
// 
// This file is part of Open Rails.
// 
// Open Rails is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Open Rails is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Open Rails.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Diagnostics;
using System.IO;
using MSTS.Parsers;
#if !NEW_SIGNALLING
using ORTS.Interlocking;
#endif

namespace MSTS.Formats
{
	/// <summary>
	/// Summary description for RDBFile.
	/// </summary>
	/// 


	public class RDBFile
	{
		public RDBFile(string filenamewithpath)
		{
			using (STFReader stf = new STFReader(filenamewithpath, false))
				stf.ParseFile(new STFReader.TokenProcessor[] {
                    new STFReader.TokenProcessor("trackdb", ()=>{ RoadTrackDB = new RoadTrackDB(stf); }),
                });
		}

		/// <summary>
		/// Provide a link to the TrJunctionNode for the switch track with 
		/// the specified UiD on the specified tile.
		/// 
		/// Called by switch track shapes to determine the correct position of the points.
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileZ"></param>
		/// <param name="UiD"></param>
		/// <returns></returns>
		public TrJunctionNode GetTrJunctionNode(int tileX, int tileZ, int UiD)
		{
			foreach (TrackNode tn in RoadTrackDB.TrackNodes)
				if (tn != null && tn.TrJunctionNode != null)
				{
					if (tileX == tn.UiD.WorldTileX
						&& tileZ == tn.UiD.WorldTileZ
						&& UiD == tn.UiD.WorldID)
						return tn.TrJunctionNode;
				}
			throw new InvalidDataException("RDB Error, could not find junction. (tileX = " + tileX.ToString() +
				", tileZ = " + tileZ.ToString() + ", UiD = " + UiD.ToString() + ")");
		}

		public RoadTrackDB RoadTrackDB;  // Warning, the first RDB entry is always null
	}



	public class RoadTrackDB
	{
		public RoadTrackDB(STFReader stf)
		{
			stf.MustMatch("(");
			stf.ParseBlock(new STFReader.TokenProcessor[] {
                new STFReader.TokenProcessor("tracknodes", ()=>{
                    stf.MustMatch("(");
                    int count = stf.ReadInt(null);
                    TrackNodes = new TrackNode[count + 1];
                    int idx = 1;
                    stf.ParseBlock(new STFReader.TokenProcessor[] {
                        new STFReader.TokenProcessor("tracknode", ()=>{ TrackNodes[idx] = new TrackNode(stf, idx, count); ++idx; }),
                    });
                }),
                new STFReader.TokenProcessor("tritemtable", ()=>{
                    stf.MustMatch("(");
                    int count = stf.ReadInt(null);
                    TrItemTable = new TrItem[count];
                    int idx = -1;
                    stf.ParseBlock(()=> ++idx == -1, new STFReader.TokenProcessor[] {
                        new STFReader.TokenProcessor("levelcritem", ()=>{ TrItemTable[idx] = new RoadLevelCrItem(stf,idx); }),
                        new STFReader.TokenProcessor("emptyitem", ()=>{ TrItemTable[idx] = new EmptyItem(stf,idx); }),
                        new STFReader.TokenProcessor("carspawneritem", ()=>{ TrItemTable[idx] = new CarSpawnerItem(stf,idx); })
                    });
                }),
            });
		}
		public TrackNode[] TrackNodes;
		public TrItem[] TrItemTable;


		public int TrackNodesIndexOf(TrackNode targetTN)
		{
			for (int i = 0; i < TrackNodes.Length; ++i)
				if (TrackNodes[i] == targetTN)
					return i;
			throw new InvalidOperationException("Program Bug: Can't Find Track Node");
		}
	}




	public class RoadLevelCrItem : TrItem
	{
		public uint Direction;                // 0 or 1 depending on which way signal is facing
		public int sigObj = -1;               // index to Sigal Object Table
		public int revDir
		{
			get { return Direction == 0 ? 1 : 0; }
		}

		public RoadLevelCrItem(STFReader stf, int idx)
		{
			ItemType = trItemType.trXING;
			stf.MustMatch("(");
			stf.ParseBlock(new STFReader.TokenProcessor[] {
                new STFReader.TokenProcessor("tritemid", ()=>{ TrItemID(stf, idx); }),
                new STFReader.TokenProcessor("tritemrdata", ()=>{ TrItemRData(stf); }),
                new STFReader.TokenProcessor("tritemsdata", ()=>{ TrItemSData(stf); }),
                new STFReader.TokenProcessor("tritempdata", ()=>{ TrItemPData(stf); })

            });
		}
	}

	public class CarSpawnerItem : TrItem
	{
		public CarSpawnerItem(STFReader stf, int idx)
		{
			ItemType = trItemType.trCarSpawner;
			stf.MustMatch("(");
			stf.ParseBlock(new STFReader.TokenProcessor[] {
                new STFReader.TokenProcessor("tritemid", ()=>{ TrItemID(stf, idx); }),
                new STFReader.TokenProcessor("tritemrdata", ()=>{ TrItemRData(stf); }),
                new STFReader.TokenProcessor("tritemsdata", ()=>{ TrItemSData(stf); })
            });
		}
	}

	//test

}