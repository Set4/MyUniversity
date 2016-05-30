package md52e96339475bff477096b782f51686822;


public class AuthintificationActivity
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("MyUniversity.Android.AuthintificationActivity, MyUniversity.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", AuthintificationActivity.class, __md_methods);
	}


	public AuthintificationActivity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == AuthintificationActivity.class)
			mono.android.TypeManager.Activate ("MyUniversity.Android.AuthintificationActivity, MyUniversity.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
