
	public class Singleton<T>
	{
		/// <summary>
		/// 单例实例
		/// </summary>
		public static Singleton<T> Instance;
		/// <summary>
		/// 私有化构造函数
		/// </summary>
		protected  Singleton()
		{

		}
		/// <summary>
		/// 创建单例实例
		/// </summary>
		/// <returns></returns>
		public Singleton<T> CreateInstance()
		{
			if (Instance == null)
				return new Singleton<T>();
			else
				return Instance;
		}
	}
