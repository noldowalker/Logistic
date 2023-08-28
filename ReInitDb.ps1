cd .\Logistic\Logistic.WebApi\

function Run-Command {
    param (
        [string]$command,
        [string]$description
    )

    Write-Host -NoNewline $description
    Invoke-Expression $command > $null
    if ($LASTEXITCODE -ne 0) {
        Write-Host " НЕУСПЕШНО!" -ForegroundColor Red
        cd ..
        Write-Host "++ Часть операций не были завершены, процесс остановлен. ++" -ForegroundColor Yellow
        exit
    } else {
        Write-Host " УСПЕШНО!" -ForegroundColor Green
    }
}

Write-Host "`n++ Пересоздаем базу данных с зачисткой всех миграций ++`n" -ForegroundColor Yellow

Run-Command "dotnet ef database update 0" -description "откатываем миграции... "
Run-Command "dotnet ef migrations remove --startup-project ../Logistic.WebApi --project ../Logistic.Infrastructure" -description "удаляем миграции... "
Run-Command 'dotnet ef migrations add "InitialCreate"  --startup-project ../Logistic.WebApi --project ../Logistic.Infrastructure' -description "пересоздаем стартовую миграцию... "
Run-Command "dotnet ef database update" -description "применяем стартовую миграцию... "

Write-Host "`n++ Успешно завершено ++`n" -ForegroundColor Yellow
cd ..\..

