# Удаление учетной записи JudgeRunner
$Username = "JudgeRunner"

Write-Host "Удаление учетной записи $Username..." -ForegroundColor Yellow

# Удаление правил Firewall
Remove-NetFirewallRule -DisplayName "Block JudgeRunner Outbound" -ErrorAction SilentlyContinue
Remove-NetFirewallRule -DisplayName "Block JudgeRunner Inbound" -ErrorAction SilentlyContinue

# Удаление учетной записи
Remove-LocalUser -Name $Username -ErrorAction SilentlyContinue

Write-Host "Учетная запись удалена" -ForegroundColor Green
