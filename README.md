# Automação para Recursos Humanos

Esta aplicação realiza o processamento de arquivos .CSV em lote e os tranforma em um único arquivo .json.

Fiz uma alteração no padrão do JSON esperado, coloquei uma estrutura com os possíveis erros que possam ocorrer devido a problemas no nome do arquivo ou na estrutura do CSV. 

## Executando a aplicação

### É obrigatório que a porta 80 esteja liberada

1. Fazer pull do projeto
2. Abra um prompt de comando no dirétorio `Infra`
3. Execute o comando `docker-compose build` 
4. Execute o comando `docker-compose up` 
5. Cole no navegador o link `http://localhost`
