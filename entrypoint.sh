#!/bin/bash

set -e
run_cmd="dotnet watch run --urls http://*:4000 --project simple-chat-app/SimpleChatApp.csproj"

export PATH="$PATH:/root/.dotnet/tools"

until dotnet ef --startup-project "simple-chat-app/SimpleChatApp.csproj" database update; do
    >&2 echo "Migrations executing"
    sleep 1
done

>&2 echo "DB Migrations complete, starting app."
>&2 echo "Running': $run_cmd"
exec $run_cmd