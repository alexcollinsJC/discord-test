rm -rf  /home/runner/dotnet
mkdir -p  /home/runner/dotnet
cd /home/runner/dotnet

wget https://download.visualstudio.microsoft.com/download/pr/ede8a287-3d61-4988-a356-32ff9129079e/bdb47b6b510ed0c4f0b132f7f4ad9d5a/dotnet-sdk-6.0.101-linux-x64.tar.gz

tar -zxf dotnet-sdk-6.0.101-linux-x64.tar.gz

export DOTNET_ROOT=/home/runner/dotnet
export PATH=$PATH:/home/runner/dotnet