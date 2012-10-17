<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Hotelier</title>
</head>
<body>
	<div>
		<% foreach(var hotel in (IEnumerable<Hotelier.Room>)Model) { %>
			<div class="hotel">
				<p class="number"><%= hotel.Number %></p>
				<a href="<%= String.Format("/room/{0}", hotel.Id) %>">View</a>
			</div>
		<% } %>
	</div>
</body>
