using CommonLib.Network;
using System;
using System.Collections.Generic;

namespace GameCommon
{
	/// <summary>
	/// 遊戲基礎封包
	/// </summary>
	public class GameBasePacket : CommonBasePacket
	{

	}

	#region server
	public class RegisterSubServerPacket : GameBasePacket
	{
		public ServerTypeEnum m_serverType;
		public Guid m_serverId;
		public int m_clientPort;

		public RegisterSubServerPacket()
		{
			m_type = (int)GamePacketType.RegisterSubServer;
		}
	}
	public class RegisterSubServerResultPacket : GameBasePacket
	{
		public enum ResultEnum
		{
			Ok,
			Failed,
			Registered
		}
		public ServerTypeEnum m_serverType;
		public Guid m_serverId;
		public int m_clientPort;
		public ResultEnum m_result;

		public RegisterSubServerResultPacket()
		{
			m_type = (int)GamePacketType.RegisterSubServerResult;
		}
	}

	public class RegisterAccountPacket : GameBasePacket
	{
		public string m_clientSessionId;
		public string m_account;
		public string m_password;

		public RegisterAccountPacket()
		{
			m_type = (int)GamePacketType.RegisterAccount;
		}
	}
	public class RegisterAccountResultPacket : GameBasePacket
	{
		public enum ResultEnum
		{
			/// <summary>
			/// 成功
			/// </summary>
			Ok,
			/// <summary>
			/// 失敗
			/// </summary>
			Failed,
			/// <summary>
			/// 帳號不合法
			/// </summary>
			InvalidAccount,
			/// <summary>
			/// 密碼不合法
			/// </summary>
			InvalidPassword,
			/// <summary>
			/// 已註冊
			/// </summary>
			Registered
		}
		public string m_clientSessionId;
		public ResultEnum m_result;

		public RegisterAccountResultPacket()
		{
			m_type = (int)GamePacketType.RegisterAccountResult;
		}
	}
	#endregion

	#region login
	public class LoginPacket : GameBasePacket
	{
		public string m_clientSessionId;
		public string m_account;
		public string m_password;

		public LoginPacket()
		{
			m_type = (int)GamePacketType.Login;
		}
	}
	public class LoginResultPacket : GameBasePacket
	{
		public enum ResultEnum
		{
			/// <summary>
			/// 成功
			/// </summary>
			Ok,
			/// <summary>
			/// 失敗
			/// </summary>
			Failed,
			/// <summary>
			/// 找不到RegionServer
			/// </summary>
			NoRegion
		}
		public string m_clientSessionId;
		public string m_regionAddress;
		public int m_regionPort;
		public Guid m_playerKey;
		public ResultEnum m_result;

		public LoginResultPacket()
		{
			m_type = (int)GamePacketType.LoginResult;
		}
	}

	public class LogoutPacket : GameBasePacket
	{
		public Guid m_playerKey;

		public LogoutPacket()
		{
			m_type = (int)GamePacketType.Logout;
		}
	}

	public class EnterRegionPacket : GameBasePacket
	{
		public string m_clientSessionId;
		public Guid m_playerKey;

		public EnterRegionPacket()
		{
			m_type = (int)GamePacketType.EnterRegion;
		}
	}
	public class EnterRegionResultPacket : GameBasePacket
	{
		public enum ResultEnum
		{
			/// <summary>
			/// 成功
			/// </summary>
			Ok,
			/// <summary>
			/// 失敗
			/// </summary>
			Failed
		}
		public string m_clientSessionId;
		public PlayerData m_playerData;
		public ResultEnum m_result;

		public EnterRegionResultPacket()
		{
			m_type = (int)GamePacketType.EnterRegionResult;
		}
	}
	#endregion

	#region player
	public class SavePlayerDataPacket : GameBasePacket
	{
		public PlayerData m_playerData;

		public SavePlayerDataPacket()
		{
			m_type = (int)GamePacketType.SavePlayerData;
		}
	}

	public class CreateRolePacket : GameBasePacket
	{
		public Guid m_playerKey;
		public string m_name;
		public List<byte> m_image;

		public CreateRolePacket()
		{
			m_type = (int)GamePacketType.CreateRole;
		}
	}
	public class CreateRoleResultPacket : GameBasePacket
	{
		public enum ResultEnum
		{
			/// <summary>
			/// 成功
			/// </summary>
			Ok,
			/// <summary>
			/// 失敗
			/// </summary>
			Failed,
			/// <summary>
			/// 名稱不合法
			/// </summary>
			InvalidName,
			/// <summary>
			/// 沒有角色空間
			/// </summary>
			NoRoleSpace
		}
		public RoleData m_roleData;
		public ResultEnum m_result;

		public CreateRoleResultPacket()
		{
			m_type = (int)GamePacketType.CreateRoleResult;
		}
	}

	public class DeleteRolePacket : GameBasePacket
	{
		public Guid m_playerKey;
		public Guid m_roleId;

		public DeleteRolePacket()
		{
			m_type = (int)GamePacketType.DeleteRole;
		}
	}
	public class DeleteRoleResultPacket : GameBasePacket
	{
		public enum ResultEnum
		{
			/// <summary>
			/// 成功
			/// </summary>
			Ok,
			/// <summary>
			/// 失敗
			/// </summary>
			Failed,
			/// <summary>
			/// 沒有角色
			/// </summary>
			NoCharacter
		}
		public Guid m_roleId;
		public ResultEnum m_result;

		public DeleteRoleResultPacket()
		{
			m_type = (int)GamePacketType.DeleteRoleResult;
		}
	}
	#endregion

	#region System
	public class ClientLogPacket : GameBasePacket
	{
		public CommonLib.Utility.Logger.LogTypeEnum m_logType;
		public string m_message;

		public ClientLogPacket()
		{
			m_type = (int)GamePacketType.ClientLog;
		}
	}
	#endregion
}
