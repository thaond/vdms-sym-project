<%@ Page Language="C#" EnableTheming="false" EnableViewState="false" Theme="" %>
<script runat="server">
	protected void Page_Load(object sender, EventArgs e)
	{
		Response.ContentType = "text/html";
		Response.Write("alive");
	}
</script>
