1. The Dockerfile is used to build images while the docker-compose.yaml file is used to run images. 
2. The Dockerfile uses the docker build command to build image. -t to give name and version if image. And use . to specify the image context
		docker build -t efcodefirstsample:v1 .
3. while the docker-compose.yaml file uses the docker-compose up command.
	docker-compose up
4. Download sql image using command
	docker pull mcr.microsoft.com/mssql/server:2022-latest
5. Run docker image using command
	docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=sapass@123" -p 1433:1433 --name localsql -d -v sqlvolume:/var/opt/mssql mcr.microsoft.com/mssql/server:2022-latest
	where localsql is container name, -v specify volume to persist sql data in containers
6. To use sql in command line, first connect with container
	docker exec -it localsql "bash"
	where localsql is container name and then make connection using sql command line
	use /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P 'sapass@123'
	To change sql password for sa use command
	docker exec -it localsql /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P 'sapass@123' -Q "ALTER LOGIN SA WITH PASSWORD='newpassword'"

