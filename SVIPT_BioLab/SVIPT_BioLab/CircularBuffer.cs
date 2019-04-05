using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVIPT_BioLab
{
    //Implementa um buffer circular para armazenamento de dados (tipo de dados: byte).
    //O tamanho do buffer circular deve ser definido no construtor, conforme
    //as necessidades do processo.
    public class CircularBufferBytes
    {
        private int sizeBuffer;
        private byte[] buffer;
        private int pWr; //ponteiro escrita
        private int pRd; //ponteiro leitura
        private int countUsed; //contador de posições ocupadas

        public CircularBufferBytes(int size)
        {
            sizeBuffer = size;
            buffer = new byte[size];
            pWr = 0;
            pRd = 0;
            countUsed = 0;
        }

        //Escreve um novo valor no buffer. 
        //Retorna true se a operação foi feita com sucesso.
        //Se não houver espaço para nova amostra, retorna falso.
        public bool write(byte val)
        {

            countUsed = count();
            if (countUsed < sizeBuffer)
            {
                buffer[pWr] = val;
                pWr++;
                if (pWr >= sizeBuffer) pWr = 0; //circular
                countUsed = count();
                return true;
            }
            else
                return false;
        }

        //Retorna o próximo valor do buffer
        public byte read()
        {
            //ATENÇÃO. Como neste código não estou retornando erro no caso de não existir dado para leitura
            //A função que chama este método deve verificar se existe algo para ler (count) antes de chamar este método;
            byte val = 0;
            countUsed = count();
            if (countUsed > 0)
            {
                val = buffer[pRd];
                pRd++;
                if (pRd >= sizeBuffer) pRd = 0; //circular
                countUsed = count();
                return val;
            }
            else
                return val;
        }

        //retorna a quantidade de amostras no buffer que podem ser lidas (ocupação do buffer)
        public int count()
        {
            if (pRd < pWr)
                countUsed = (pWr - pRd);

            else if (pRd > pWr)
                countUsed = (sizeBuffer - pRd + pWr);

            else
                countUsed = 0;
            return countUsed;
        }

        //retorna o tamanho do buffer circular
        public int size()
        {
            return sizeBuffer;
        }

        //Verifica se 'thisByte' existe no buffer circular. A busca se iniciará
        //'offset' posições a partir do ponteiro de leitura atual.
        //Retorno:
        //      -1: 'thisByte' não existe no buffer circular.
        //      n >= 0 :offset, a partir do ponteiro de leitura atual, da primeira
        //              ocorrência de 'thisByte'.
        public int containByte(byte thisByte, int offset)
        {
            int pRD1, contMax, i;

            //varre os countUsed dados no buffer a partir de pRd
            pRD1 = pRd; //não mudar o ponteiro de leitura.
            contMax = countUsed; //countUsed pode mudar durante varredura por conta de acesso assincrono em threads etc.
            contMax = contMax - offset; //descontar 'offset'

            if (contMax <= 0)
                return -1;

            for (i = 0; i < contMax; i++)
            {
                if (buffer[pRD1 + offset] == thisByte)
                {
                    //buffer circular contém 'thisByte'.
                    return (i + offset);
                }
                pRD1++; //continua busca
                if (pRD1 >= sizeBuffer) //circular buffer
                    pRD1 = 0;

            }
            return -1; //buffer circular não contém 'thisByte'.
        }

        public void clear()
        {
            pWr =0; //ponteiro escrita
            pRd =0; //ponteiro leitura
            countUsed =0;
        }
        //Verifica se a sequência 'theseBytes' existe no buffer circular. 
        //A busca se iniciará 'offset' posições a partir do ponteiro de leitura atual.
        //nBytes: quantidade de elementos do array.
        //Retorno:
        //      -1: a sequência 'theseBytes' não existe no buffer circular.
        //      n >= 0 :offset, a partir do ponteiro de leitura atual, da primeira
        //              ocorrência da sequência 'theseBytes'.
        public int containBytes(byte[] theseBytes, int nBytes, int offset)
        {
            int pRd1, pRd2, contMax, i, j;
            bool next;

            //varre os countUsed dados no buffer a partir de pRd
            pRd1 = pRd; //não mudar o ponteiro de leitura.
            contMax = countUsed; //countUsed pode mudar durante varredura por conta de acesso assincrono em threads etc.
            contMax = contMax - offset; //descontar 'offset'

            if (contMax < nBytes) //pelo menos a quantidade de bytes de theseBytes.
                return -1;
            //varrer os bytes (pode para quando não houver mais nBytes a serm verificados)
            for (i = 0; i <= (contMax - nBytes); i++)
            {
                if (buffer[pRd1 + offset] == theseBytes[0])
                {
                    //primeiro byte de theseBytes localizado. E os demais?
                    pRd2 = pRd1 + 1;
                    next = true;
                    for (j = 1; j < nBytes; j++)
                    {
                        if (buffer[pRd2 + offset] != theseBytes[j])
                        {
                            next = false;
                            break; //stop for j
                        }
                        pRd2++; //continua busca por theseBytes
                        if (pRd2 >= sizeBuffer) //circular buffer
                            pRd2 = 0;
                    }
                    if (next)
                        return (i + offset); //buffer circular contém 'theseBytes'.
                }
                pRd1++; //continua busca
                if (pRd1 >= sizeBuffer) //circular buffer
                    pRd1 = 0;
            }
            return -1; //buffer circular não contém 'theseBytes'.
        }
    }
}
