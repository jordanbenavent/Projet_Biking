start ./ProxySOAPExecutable\ProxySOAPExecutable\bin\Debug\ProxySOAPExecutable.exe

start ./ServeurBikingExecutable\ServeurBikingExecutable\bin\Debug\ServeurBikingExecutable.exe 

start activemq start 

cd ClientJava\client && mvn clean package && mvn exec:java -e