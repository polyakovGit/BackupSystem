# BackupSystem
# Сначала запуск server.exe, затем desktopClient.exe
# если проблемы со службой резервирования то sc stop test, sc delete test и снова запуск desktopClient.exe
# для запуска desktopClient.exe в том же каталоге должен быть путь ClientService/ClientService.exe, 
# так как подчиненная служба управляет передачей пакетов, а она  #создается в desktopClient, для чего нужны права администратора при запуске.
