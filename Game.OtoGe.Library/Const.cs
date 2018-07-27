using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.OtoGe.Library
{
	public static class Const
	{
		/// <summary>
		/// ボードの横幅
		/// </summary>
		public const float BoardWidth = 1820;
		/// <summary>
		/// ボードの横幅の半分
		/// </summary>
		public const float BoardWidthHalf = BoardWidth / 2;
		/// <summary>
		/// 1tickで動く幅
		/// </summary>
		public const float MovePointInTick = 0.9f;//0.4f
		/// <summary>
		/// 音符生成位置の調整 //TODO:感覚的に調整
		/// </summary>
		public const float NoteXAdjustment = 90f;
		/// <summary>
		/// 判定中心線のX座標
		/// </summary>
		public const float JudgeLineX = -750;
		/// <summary>
		/// 1拍あたりのTick数
		/// </summary>
		public const int TickInTempo = 480;
		/// <summary>
		/// テンポが指定されていない場合のテンポ
		/// </summary>
		public const int DefaultTempo = 120;
	}

}