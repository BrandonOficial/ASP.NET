# Projeto DESAFIOCRUD

`DESAFIOCRUD` é uma aplicação web ASP.NET Core MVC (.NET 9) que implementa um CRUD (Create, Read, Update, Delete) completo para gerenciamento de Fornecedores.

O projeto utiliza Entity Framework Core para persistência de dados em um banco de dados MySQL e inclui uma funcionalidade de upload de imagens (fotos dos fornecedores) em formato PNG.

## ✨ Features Principais

* **Listagem de Fornecedores** (Index)
* **Criação de Fornecedor** (Create)
* **Edição de Fornecedor** (Edit)
* **Detalhes do Fornecedor** (Details)
* **Exclusão de Fornecedor** (Delete)
* **Upload de Fotos**: O sistema permite o upload de uma foto (obrigatóriamente `.png`) no momento da Criação e Edição de um fornecedor. As imagens são salvas em `wwwroot/imagens` e o caminho é referenciado no banco de dados.

## 🛠️ Tecnologias Utilizadas

* **.NET 9**
* **ASP.NET Core MVC**
* **Entity Framework Core 9**
* **MySQL** (utilizando o provider `Pomelo.EntityFrameworkCore.MySql`)
* **Injeção de Dependência** (para `ApplicationDbContext` e `IWebHostEnvironment`)

## 🏗️ Estrutura do Projeto

Aqui estão os arquivos-chave que definem a arquitetura da aplicação:

### 1. `Models/Fornecedor.cs`
A entidade principal do projeto. Define os dados de um Fornecedor, incluindo validações (Data Annotations) para campos como `Nome`, `Cnpj` (com 14 dígitos numéricos) e `Cep`.

* `Segmento`: É um `Enum` que vem do arquivo `Models/segmento.cs`.
* `FotoCaminho`: Armazena o *caminho* da imagem no servidor (ex: `/imagens/foto.png`).
* `FotoArquivo`: Propriedade `[NotMapped]` do tipo `IFormFile` usada exclusivamente para receber o arquivo durante o upload no formulário.

### 2. `Data/ApplicationDbContext.cs`
O contexto do Entity Framework Core. É responsável pela comunicação com o banco de dados.
* Mapeia a entidade `Fornecedor` para uma tabela chamada `Fornecedores` no banco de dados.

### 3. `Controllers/FornecedoresController.cs`
O coração da aplicação. Este controlador gerencia todas as ações HTTP para o CRUD de Fornecedores.
* Recebe `ApplicationDbContext` e `IWebHostEnvironment` via injeção de dependência.
* **`Create()` (POST)**:
    * Valida o `ModelState`.
    * Verifica se um `FotoArquivo` foi enviado e se é do tipo `image/png`.
    * Gera um nome único (`Guid`) para o arquivo.
    * Salva o arquivo fisicamente na pasta `wwwroot/imagens` (criando a pasta se ela não existir).
    * Salva o caminho (ex: `/imagens/nome-unico.png`) na propriedade `FotoCaminho` do modelo.
    * Adiciona o fornecedor ao banco e salva as mudanças.
* **`Edit()` (POST)**:
    * Possui lógica similar ao `Create` para lidar com a *troca* da foto.
    * Se nenhum novo arquivo for enviado (`FotoArquivo == null`), ele preserva o `FotoCaminho` existente (que vem de um `<input type="hidden">`).

### 4. `Program.cs`
Arquivo de inicialização da aplicação (entry point).
* Lê a string de conexão `DefaultConnection` do `appsettings.json`.
* Registra o `ApplicationDbContext` no container de injeção de dependência, configurando o provider do `Pomelo` para `UseMySql`.

### 5. `appsettings.json`
Arquivo de configuração principal.
* Define a `DefaultConnection` para o banco de dados MySQL.
* **Configuração Padrão:** `Server=localhost;Database=desafiocrud_db;Uid=root;Pwd=root;`

## 🚀 Como Executar o Projeto

Bora rodar esse projeto!

### Pré-requisitos
1.  [.NET 9 SDK](https://dotnet.microsoft.com/download) (ou superior).
2.  Um servidor MySQL local (ou um container Docker com MySQL).

### 1. Configurar o Banco de Dados
O projeto está configurado para se conectar a um banco local.

1.  Verifique se seu servidor MySQL está rodando.
2.  Crie um banco de dados com o nome `desafiocrud_db`:
    ```sql
    CREATE DATABASE desafiocrud_db;
    ```
3.  Fica de olho: Se o seu usuário e senha do MySQL não forem `root` / `root`, atualize a `DefaultConnection` no arquivo `appsettings.json`.

### 2. Aplicar as Migrations
Este projeto usa EF Core Code-First. Você precisa aplicar as migrations para que o EF crie as tabelas no banco.

```bash
# Navegue até a pasta do projeto (onde está o DESAFIOCRUD.csproj)
cd /caminho/para/DESAFIOCRUD

# (Opcional) Se as migrations não existissem, você as criaria assim:
# dotnet ef migrations add VersaoInicial

# Aplica as migrations existentes ao banco de dados:
dotnet ef database update

#Após o banco de dados estar pronto e atualizado, basta executar a aplicação
dotnet run


#Abra seu navegador e acesse a URL indicada no terminal (geralmente http://localhost:5xxx ou https://localhost:7xxx). Navegue até /Fornecedores para ver o CRUD em ação.
