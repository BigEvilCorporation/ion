<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ion._Default" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1><%: Title %>.</h1>
                <h2>ion::code</h2>
            </hgroup>
            <p>
                A teeny tiny assembly language for making teeny tiny 2D games</p>
        </div>
    </section>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:TextBox ID="txtSourceCode" runat="server" Height="450px" MaxLength="140" OnTextChanged="txtEditor_TextChanged" TextMode="MultiLine" Width="566px"></asp:TextBox>
    <br />
    <asp:TextBox ID="txtAssembleLog" runat="server" Height="136px" ReadOnly="True" TextMode="MultiLine" Width="566px"></asp:TextBox>
    <asp:GridView ID="gridMemory" runat="server" OnSelectedIndexChanged="gridMemory_SelectedIndexChanged" ShowHeader="False" CellPadding="4" ForeColor="#333333" GridLines="None" Height="178px" Width="263px">
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <EditRowStyle BackColor="#999999" />
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#E9E7E2" />
        <SortedAscendingHeaderStyle BackColor="#506C8C" />
        <SortedDescendingCellStyle BackColor="#FFFDF8" />
        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
    </asp:GridView>
    <asp:Button ID="btnCompile" runat="server" OnClick="btnCompile_Click" Text="Compile" />
    <asp:Button ID="btnExecute" runat="server" OnClick="btnExecute_Click" Text="Execute" />
    <br />
    <br />
</asp:Content>
