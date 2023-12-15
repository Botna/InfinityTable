sudo systemctl stop infinityTableBlazorSite.service
dotnet build
dotnet publish -o /srv/infinityTableBlazorSite
sudo systemctl start infinityTableBlazorSite.service


