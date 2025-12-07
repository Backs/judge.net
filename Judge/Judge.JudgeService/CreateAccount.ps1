# Параметры скрипта
param(
    [Parameter(Mandatory=$true, HelpMessage="Пароль для учетной записи JudgeRunner")]
    [string]$Password,
    
    [Parameter(Mandatory=$false)]
    [string]$Username = "JudgeRunner",
    
    [Parameter(Mandatory=$false)]
    [string]$Description = "Restricted account for running contest solutions"
)

# Проверка прав администратора
if (-NOT ([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole] "Administrator")) {
    Write-Error "Этот скрипт требует прав администратора!"
    Exit 1
}

Write-Host "Создание учетной записи $Username..." -ForegroundColor Green

# Создание учетной записи
$SecurePassword = ConvertTo-SecureString $Password -AsPlainText -Force
try {
    New-LocalUser -Name $Username -Password $SecurePassword -Description $Description -AccountNeverExpires -PasswordNeverExpires -ErrorAction Stop
    Write-Host "Учетная запись создана успешно" -ForegroundColor Green
} catch {
    if ($_.Exception.Message -like "*already exists*") {
        Write-Host "Учетная запись уже существует. Обновление пароля..." -ForegroundColor Yellow
        Set-LocalUser -Name $Username -Password $SecurePassword
    } else {
        Write-Error "Ошибка создания учетной записи: $_"
        Exit 1
    }
}

# Получаем SID пользователя
$user = Get-LocalUser -Name $Username
$userSID = $user.SID.Value

# Блокировка сетевого доступа
Write-Host "Блокировка сетевого доступа..." -ForegroundColor Green

Remove-NetFirewallRule -DisplayName "Block JudgeRunner Outbound" -ErrorAction SilentlyContinue
Remove-NetFirewallRule -DisplayName "Block JudgeRunner Inbound" -ErrorAction SilentlyContinue

New-NetFirewallRule -DisplayName "Block JudgeRunner Outbound" `
    -Direction Outbound `
    -Action Block `
    -Owner $userSID `
    -Profile Any `
    -Enabled True | Out-Null

New-NetFirewallRule -DisplayName "Block JudgeRunner Inbound" `
    -Direction Inbound `
    -Action Block `
    -Owner $userSID `
    -Profile Any `
    -Enabled True | Out-Null

Write-Host "`n=== Настройка завершена ===" -ForegroundColor Cyan
Write-Host "Учетная запись: $Username" -ForegroundColor White
Write-Host "SID: $userSID" -ForegroundColor White

Exit 0
