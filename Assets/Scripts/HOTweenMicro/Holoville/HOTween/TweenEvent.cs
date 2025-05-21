using Holoville.HOTween.Plugins.Core;

namespace Holoville.HOTween
{
	public class TweenEvent
	{
		private readonly IHOTweenComponent _tween;

		private readonly object[] _parms;

		private readonly ABSTweenPlugin _plugin;

		public IHOTweenComponent tween
		{
			get
			{
				return _tween;
			}
		}

		public object[] parms
		{
			get
			{
				return _parms;
			}
		}

		public ABSTweenPlugin plugin
		{
			get
			{
				return _plugin;
			}
		}

		internal TweenEvent(IHOTweenComponent p_tween, object[] p_parms)
		{
			_tween = p_tween;
			_parms = p_parms;
			_plugin = null;
		}

		internal TweenEvent(IHOTweenComponent p_tween, object[] p_parms, ABSTweenPlugin p_plugin)
		{
			_tween = p_tween;
			_parms = p_parms;
			_plugin = p_plugin;
		}
	}
}
