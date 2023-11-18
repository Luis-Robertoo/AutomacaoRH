# Automação para Recursos Humanos

Essa aplicação tem como objetivo realizar cálculos de fechamento de ponto, mediante o envio de arquivos `.CSV`.

A aplicação realiza o processamento desses arquivos em lote e os tranforma em um único arquivo `.JSON`.

Foi realizada uma alteração no padrão do arquivo `.JSON` esperado, onde foi implementado uma estrutura com os possíveis erros que possam ocorrer, devido a inconsistências no nome do arquivo ou na estrutura dos arquivos `.CSV`. 

## Executando a aplicação

### É obrigatório que a porta 80 esteja liberada

1. Fazer pull do projeto
2. Abra um prompt de comando no dirétorio `Infra`
3. Execute o comando `docker-compose build` 
4. Execute o comando `docker-compose up` 
5. Cole no navegador o link `http://localhost`
