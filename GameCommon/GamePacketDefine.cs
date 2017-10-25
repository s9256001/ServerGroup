
namespace GameCommon
{
	/// <summary>
	/// 封包類型
	/// </summary>
	public enum GamePacketType
	{
		#region server
		/// <summary>
		/// 註冊子Server
		/// </summary>
		RegisterSubServer = 1,
		/// <summary>
		/// 註冊子Server結果
		/// </summary>
		RegisterSubServerResult,
		#endregion

		#region login
		/// <summary>
		/// 註冊帳號
		/// </summary>
		RegisterAccount = 1001,
		/// <summary>
		/// 註冊帳號
		/// </summary>
		RegisterAccountResult,
		/// <summary>
		/// 登入
		/// </summary>
		Login,
		/// <summary>
		/// 登入結果
		/// </summary>
		LoginResult,
		/// <summary>
		/// 登出
		/// </summary>
		Logout,
		/// <summary>
		/// 進入Region
		/// </summary>
		EnterRegion,
		/// <summary>
		/// 進入Region結果
		/// </summary>
		EnterRegionResult,
		#endregion

		#region player
		/// <summary>
		/// 儲存角色資料
		/// </summary>
		SavePlayerData = 1101,
		/// <summary>
		/// 建立角色
		/// </summary>
		CreateRole,
		/// <summary>
		/// 建立角色結果
		/// </summary>
		CreateRoleResult,
		/// <summary>
		/// 刪除角色
		/// </summary>
		DeleteRole,
		/// <summary>
		/// 刪除角色結果
		/// </summary>
		DeleteRoleResult,
		#endregion

		#region System
		/// <summary>
		/// Client Log命令
		/// </summary>
		ClientLog = 9999
		#endregion
	}
}
