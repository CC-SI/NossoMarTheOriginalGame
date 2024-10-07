# Estrutura de Diálogo em JSON

Este arquivo JSON contém a estrutura de grupos de diálogos para o sistema de diálogos. Ele está organizado em dois níveis principais: **Grupos de Diálogos** (`dialogueGroups`) e **Diálogos Individuais** dentro desses grupos. Cada grupo pode conter múltiplos diálogos, e cada diálogo tem um **falante** e um **texto** associado.

## Estrutura

### 1. `dialogueGroups`
Este é o nível mais alto da estrutura. Ele contém uma lista de grupos de diálogos. Cada grupo é definido pelos seguintes campos:

- `id`: Um identificador único para o grupo de diálogos. Este ID é usado para referenciar o grupo no código, permitindo iniciar diálogos específicos.
- `groupName`: Um nome descritivo para o grupo de diálogos. Isso pode ser usado para identificação humana.
- `dialogues`: Uma lista contendo os diálogos individuais deste grupo.

### 2. `dialogues`
Dentro de cada grupo de diálogos, há uma lista de objetos de diálogo. Cada diálogo possui os seguintes campos:

- `speaker`: O nome do falante neste diálogo, representando quem está falando no jogo.
- `text`: O texto do diálogo que será exibido na interface de usuário, correspondente à fala do falante.

### Exemplo Completo

```json
{
  "dialogueGroups": [
    {
      "id": "patoenterrado",
      "groupName": "Dialogo do pato enterrado",
      "dialogues": [
        {
          "speaker": "Player",
          "text": "Por que você está enterrado, pato?"
        },
        {
          "speaker": "Pato",
          "text": "Uma corrente me deixou preso aqui, por favor me ajude!!"
        }
      ]
    },
    {
      "id": "patosalvo",
      "groupName": "Dialogo do pato salvo",
      "dialogues": [
        {
          "speaker": "Pato",
          "text": "Obrigado por me salvar!!"
        }
      ]
    }
  ]
}
