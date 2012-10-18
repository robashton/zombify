<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Hotelier.HomeView>>" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Hotelier</title>
</head>
<body>
	<div>
		<% foreach(var hotel in Model) { %>
			<div class="hotel <%= hotel.Booked ? "booked" : "" %>" id='hotel-<%= hotel.Id %>'>
				<p class="number"><%= hotel.Number %></p>
				<a href="<%= String.Format("/room/{0}", hotel.Id) %>">View</a>
			</div>
		<% } %>
	</div>
</body>
