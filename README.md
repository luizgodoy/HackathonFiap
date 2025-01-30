# HACKATHON FIAP TURMA 4NETT

Este projeto é uma Web API desenvolvida em C# que fornece endpoints para gerenciamento de agendamento de consultas de uma operadora de saúde fictícia chamada Health&Med.

## Tecnologias Utilizadas

- **.NET Core** .NET 8
- **ASP.NET Core** para construção da Web API
- **Entity Framework Core** ORM para acesso a dados
- **FluentValidation** para validação dos dados de entrada da camada services
- **Swagger/OpenAPI** para documentação da API
- **RabbitMQ** que implementa uma mensageria para controle de concorrência
- **MS SQL SERVER EXPRESS** banco de dados relacional da aplicação

## Pré-requisitos

Antes de começar, certifique-se de ter instalado:

- [.NET SDK](https://dotnet.microsoft.com/download)
- [Visual Studio](https://visualstudio.microsoft.com/) ou [Visual Studio Code](https://code.visualstudio.com/)
- [SQL Server](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads) ou outro banco de dados relacional

## Como Executar o Projeto

1. **Clone o repositório**

   ```bash
   git clone https://github.com/luizgodoy/HackathonFiap.git

2. **Navegue até o diretório do projeto**

   ```bash
   cd HackathonFiap

3. **Restaurar as dependências**

   ```bash
   dotnet restore
   ```
   
4. **Configurar o banco de dados**

   Atualize a string de conexão no arquivo appsettings.json para apontar para o seu banco de dados.

   Execute as migrações para criar o banco de dados: 
   
   ```bash
   dotnet ef database update
   ```
   
5. **Execute a aplicação**

   ```bash
   dotnet run
   ```
   
   A API estará disponível em http://localhost:5000   

6. **Acessar a documentação da API**

   Abra o navegador e acesse http://localhost:5000/swagger/index.html para visualizar a documentação da API gerada pelo Swagger.   
   
## Estrutura do Projeto

   - **API:** Contém os controladores da API que lidam com as requisições HTTP.   
   - **Core:** Contém as classes de modelo que representam as entidades do sistema.   
   - **Data:** Contém o contexto do Entity Framework e as configurações de banco de dados.   
   - **Services:** Contém a lógica de negócio da aplicação.   
   - **Data\Migrations:** Contém as migrações do Entity Framework para gerenciar o esquema do banco de dados.   
   - **Application:** Console que é o consumidor da fila RabbitMQ.

## Grupo 70:   

- Anderson José Da Silva
and.jsilva@gmail.com

- Clademir Zampieri
mr.zampieri@live.com  
 
- Luiz Antonio Garcia de Godoy
luiz.godoy@tivit.com

- Ricardo do Vale Czajkowski
ricardovcza@gmail.com
