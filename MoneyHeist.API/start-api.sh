#!/bin/bash
ln -fs /usr/share/zoneinfo/Europe/Zagreb /etc/localtime
dpkg-reconfigure -f noninteractive tzdata
update-ca-certificates
dotnet MoneyHeist.API.dll