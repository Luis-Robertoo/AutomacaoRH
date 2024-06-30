# Automação para Recursos Humanos

Essa aplicação tem como objetivo realizar cálculos de fechamento de ponto, mediante o envio de arquivos `.CSV`.

A aplicação realiza o processamento desses arquivos em lote e os tranforma em um único arquivo `.JSON`.

Foi realizada uma alteração no padrão do arquivo `.JSON` esperado, onde foi implementado uma estrutura com os possíveis erros que possam ocorrer, devido a inconsistências no nome do arquivo ou na estrutura dos arquivos `.CSV`. 

## Modelo do arquivo

- Nome do arquivo contém: Nome do Departamento, Mês de vigência, Ano de vigência.
Exemplo: ‘Departamento de Operações Especiais-Abril-2022.csv’
- O arquivo contém as seguintes colunas: Código: número, Nome: Texto, Valor hora:
Dinheiro, Data: Dia do registro, Entrada: Hora do registro, Saída: Hora do registro, Almoço:
Hora de registro
- ![image](https://github.com/Luis-Robertoo/AutomacaoRH/assets/37544328/bce016e8-af06-4df4-bc45-32c22dd59f22)

## Modelo do JSON esperado
![image](https://github.com/Luis-Robertoo/AutomacaoRH/assets/37544328/920ba6b8-6ad2-4b4f-b099-a7c8b1c42d27)


## Executando a aplicação

### É obrigatório que a porta 80 esteja liberada

1. Fazer pull do projeto
2. Abra um prompt de comando no dirétorio `Infra`
3. Execute o comando `docker-compose build` 
4. Execute o comando `docker-compose up` 
5. Cole no navegador o link `http://localhost`
