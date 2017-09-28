#include <xc.h>
#include <stdlib.h>
#include <math.h>

// Configura��o dos Fuses do Microcontrolador.
#define _XTAL_FREQ	   18432000 // Cristal de 18432000 Mhz.
#pragma config FOSC  = HS   	// Oscillator Selection bits (HS oscillator: High-speed crystal/resonator on RA6/OSC2/CLKOUT and RA7/OSC1/CLKIN).
#pragma config WDTE  = OFF  	// Watchdog Timer Enable bit (WDT disabled).
#pragma config PWRTE = ON   	// Power-up Timer Enable bit (PWRT enabled).
#pragma config BOREN = OFF		// Brown-out Detect Enable bit (BOD disabled).
#pragma config LVP   = OFF  	// Low-Voltage Programming Enable bit (RB4/PGM pin has digital I/O function, HV on MCLR must be used for programming).
#pragma config CPD   = OFF  	// Data EE Memory Code Protection bit (Data memory code protection off).
#pragma config CP    = OFF  	// Flash Program Memory Code Protection bit (Code protection off).

//Vari�veis Globais de Controle.
unsigned short a = 0, c = 0;;
unsigned short fs = 0;
unsigned short freq = 0;
unsigned short ADCResult = 0;
unsigned short contagens = 0;
unsigned char Display1[5];
unsigned char *Display;
unsigned char simulacao = 0;
unsigned char contador = 0;
double tempo = 0;

//-----------------------------------------------------------------------------
void USART_Init(long BaudRate, int Mode)
{
	int BR = 0;

	// C�lculo do valor para o registrador SPBRG para uma determinada velocidade em bps.	
	if (Mode == 0)			//C�lculo para baixa velocidade.
	{
		BR = (_XTAL_FREQ / (64 * BaudRate)) - 1;
		SPBRG = BR;
	}
	else					//C�lculo para baixa velocidade.
	{
		BR = (_XTAL_FREQ / (16 * BaudRate)) - 1;
		SPBRG = BR;
	}

    // Configura��o do Registrador TXSTA.
    TXSTAbits.CSRC	= 1;	// Sele��o MASTER/SLAVE para o Modo S�ncrono.
    TXSTAbits.TX9	= 0;	// Transmi��o de Dados em 8 Bits.
    TXSTAbits.TXEN	= 1; 	// Habilita a Transmi��o de Dados.
    TXSTAbits.SYNC	= 0; 	// Modo de Comunica��o Ass�ncrono.
    TXSTAbits.BRGH	= Mode;	// Baud Rate em alta ou baixa velocidade.
    TXSTAbits.TRMT	= 1;	// Situa��o do Registrador Interno de Transmiss�o (TSR).
    TXSTAbits.TX9D	= 0;	// Valor a Ser Transmitido como 9� bit (Paridade/Endere�amento).

    // Configura��o do Registrador RCSTA.
    RCSTAbits.SPEN	= 1;	// Habilita o Sistema USART.
    RCSTAbits.RX9	= 0;    // Recep��o de Dados em 8 Bits.
    RCSTAbits.SREN	= 0;	// Desabilita Recep��o Unit�ria (Somente Modo S�ncrono em MASTER).
    RCSTAbits.CREN	= 1;   	// Habilita a Recep��o Cont�nua de Dados.
    RCSTAbits.ADDEN	= 0;  	// Desabilita o Sistema de Endere�amento.
    RCSTAbits.FERR	= 0;	// Erro de Stop Bit.
    RCSTAbits.OERR	= 0;	// Erro de Muitos Bytes Recebidos sem Leitura.
    RCSTAbits.RX9D	= 0;	// Valor a Ser Recebido como 9� bit (Paridade/Endere�amento).

    // Configura��o da Interrup��o USART.
	PIE1bits.RCIE 	= 1;	// Habilita a Interrup��o Serial.
	PIR1bits.RCIF 	= 0;	// Habilita a Interrup��o Serial de Recep��o.
}
//-----------------------------------------------------------------------------
void USART_WriteChar(unsigned char USARTData)
{
	while(!PIR1bits.TXIF);
	TXREG = USARTData;
}
//-----------------------------------------------------------------------------
void USART_WriteString(const char *str)
{
	// Efetua a transmiss�o da string para a USART.
	while(*str != '\0')
  	{
    	// Envio da string byte a byte.
		USART_WriteChar(*str);
      	str++;
  	}
}
//-----------------------------------------------------------------------------
unsigned char USART_ReceiveChar(void)
{
	unsigned char USARTData;

	__delay_us(75);					// Tempo necess�rio para recep��o do pr�ximo byte.
	
	if (!OERR)						// Erro de Muitos Bytes Recebidos sem Nenhuma Leitura.
	{
		USARTData 		= RCREG;	// Recebe o byte da USART e atribui a vari�vel USARTData.
    	PIR1bits.RCIF 	= 0;		// Limpa a Flag da Interrup��o de Recep��o.
	}
	else
	{
		USART_WriteString("\n\r ------- ESTOURO DE PILHA ------- \n\r ");

		RCSTAbits.CREN 	= 0;   		// Desabilita a Recep��o Cont�nua de Dados Momentaneamente.
		USARTData 		= RCREG;	// Recebe o byte da USART e atribui a vari�vel USARTData.
		RCSTAbits.CREN 	= 1;   		// Habilita a Recep��o Cont�nua de Dados Novamente.
    	PIR1bits.RCIF 	= 0;		// Limpa a Flag da Interrup��o de Recep��o.				
	}
	
	return(USARTData);
}
//-----------------------------------------------------------------------------
void ADC_Init()
{
	//Configura��o do Registrador ADCON1 para a Convers�o A/D.
	ADCON1bits.ADFM  = 1;
	ADCON1bits.PCFG3 = 0;
	ADCON1bits.PCFG2 = 0;
	ADCON1bits.PCFG1 = 0;
	ADCON1bits.PCFG0 = 0;

	//Configura��o do Registrador ADCON0 para a Convers�o A/D.
	ADCON1bits.ADCS2 = 1;
	ADCON0bits.ADCS1 = 1;
	ADCON0bits.ADCS0 = 0;

	//Configura��o dos Registradores PIE1 e PIR1 para a Convers�o A/D.
	PIE1bits.ADIE = 1;
    PIR1bits.ADIF = 0;
}
//-----------------------------------------------------------------------------
void ADC_Read(unsigned short channel)
{
	ADCON0bits.CHS = channel;	// Configura��o do canal a ser lido (mesmo que CHS2:CHS1:CHS0 em bits).
	ADCON0bits.ADON	= 1;		// Ativa o m�dulo do conversor A/D.

	__delay_us(25); 			// Espera o tempo requerido para adequa��o do conversor A/D.

	ADCON0bits.GO = 1;			// Ativa o sistema de convers�o de dados.
	while(ADCON0bits.GO_DONE);	// Espera at� que o dado lino no canal seja convertido.
    
	//PIR1bits.ADIF = 0;			// Caso esteja utilizando a interrup��o A/D, limpa a flag para nova convers�o.
}
//-----------------------------------------------------------------------------
void interrupt ISR(void)
{
	// Verifica se a interrup��o foi causada pela recep��o de bytes.
	if (PIR1bits.RCIF)		
    {
		if (USART_ReceiveChar() == '#')
		{
			switch(USART_ReceiveChar())
			{
				case 'S': 
					// Inicializa��o do TIMER1.
					T1CONbits.T1OSCEN = 0;  // Bit para escolher o prescaler.
					T1CONbits.T1CKPS1 = 1;  // Bit para escolher o prescaler.
					T1CONbits.T1CKPS0 = 1;  // Bit para escolher o prescaler.
					T1CONbits.TMR1ON = 1;	// Bit que aciona o TIMER1.

					switch(USART_ReceiveChar())
					{
						case '1':
							fs = 100;
							contagens = 59776;	// Valor inicial para in�cio de contagem (100Hz - 10ms).
							TMR1 = contagens;
						break;

						case '2':
							fs = 250;
							contagens = 63232;	// Valor inicial para in�cio de contagem (250Hz - 4ms).
							TMR1 = contagens;
						break;

						case '3':
							fs = 500;
							contagens = 64384;	// Valor inicial para in�cio de contagem (500Hz - 2ms).
							TMR1 = contagens;
						break;

						case '4':
							fs = 1000;
							contagens = 64960;		// Valor inicial para in�cio de contagem (1000Hz - 1ms).
							TMR1 = contagens;
						break;

						case 'S':
							fs = 200;
							contagens = 62656;		// Valor inicial para in�cio de contagem (200Hz - 5ms).
							TMR1 = contagens;
							simulacao = 1;
							tempo = 0;
						break;
					}

					PIE1bits.TMR1IE = 1;	

					a = 0;			
				break;

				case 'C': 
					switch(USART_ReceiveChar())
					{
						case '1':
							freq = 1.0; USART_WriteString("1.0 Hz\n");
						break;

						case '2':
							freq = 5.0; USART_WriteString("5.0 Hz\n");
						break;

						case '3':
							freq = 10.0; USART_WriteString("10.0 Hz\n");
						break;

						case '4':
							freq = 20.0; USART_WriteString("20.0 Hz\n");
						break;

						case '5':
							freq = 60.0; USART_WriteString("60.0 Hz\n");
						break;

						case '6':
							freq = 100.0; USART_WriteString("100.0 Hz\n");
						break;

						case '7':
							freq = 200.0; USART_WriteString("200.0 Hz\n");
						break;
					}
				break;

				case 'P': 					
					T1CONbits.TMR1ON = 0;
					PIE1bits.TMR1IE = 0;	
					simulacao = 0;		
					tempo = 0;	
 				break;
			}
		}
	}

	// Verifica se a interrup��o foi causada pelo TIMER1.
	if (PIR1bits.TMR1IF)
	{

		RB4=1;			

		if (simulacao)
		{
			unsigned char low, high;
    		unsigned int inteiro = 0;
			double amostra = 0;
			
			amostra = (sin(2.0 * 3.14159265358979323 * freq * tempo) * 2.5) + 2.5;
			tempo = tempo + (1.0 / fs);
			inteiro = (unsigned int) (amostra / 0.004887586);

				high = inteiro >> 8;
				low = inteiro;

				if (inteiro == 48)
				{
					high = 0x11;
					low = 0x11;
				}
				
				if (inteiro == 304)
				{
					high = 0x22;
					low = 0x22;
				}

				if (inteiro == 560)
				{
					high = 0x33;
					low = 0x33;
				}
				if (inteiro == 816)
				{
					high = 0x44;
					low = 0x44;
				}

				if (high == '\0') 
				{
					USART_WriteChar('0');
				}
				else
				{
					USART_WriteChar('[');	
					USART_WriteChar(high);	
					USART_WriteChar(']');	
				}

				if (low == '\0') 
				{	
					USART_WriteChar('0');	
				}	
				else
				{
					USART_WriteChar('{');	
					USART_WriteChar(low);	
					USART_WriteChar('}');	
				}
				
		    if (a == fs)
			{

				tempo = 0;	
				amostra = 0;
				inteiro = 0;
				a = 0;
			}
			else
			{
				a++;
			}
		}
		else
		{
			ADC_Read(0);	// Leitura do canal 0.
		}
		
		TMR1 = contagens;	// Recarrega o valor para uma nova contagem (100ms-7936, 10ms-59776, 5ms-62656, 4ms-63232, 2ms-64384, 1ms-64960, 500us-65248).

		PIR1bits.TMR1IF = 0; 	// Limpa a flag da interrup��o do TIMER1.

	RB4=0;

}

	// Verifica��o se a Interrup��o foi causada pela convers�o A/D.
	if (PIR1bits.ADIF)
	{
		if (a == 0)
		{
			USART_WriteChar('$');
		}

		if (ADRESH == '\0') 
		{
			USART_WriteChar('0');
		}
		else
		{
			USART_WriteChar(ADRESH);	
		}

		if (ADRESL == '\0') 
		{
			USART_WriteChar('0');	
		}
		else
		{
			USART_WriteChar(ADRESL);	
		}
	
		if (a == (fs - 1))
		{	
			a = 0;
		}
		else
		{
			a++;
		}

		PIR1bits.ADIF = 0;	// Limpa a flag da interrup��o do conversor A/D.
	}
}

//-----------------------------------------------------------------------------
void main(void)
{
    TRISA	= 0b11111111;	// Configura��o dos canais anal�gicos do PORTA.
    PORTA	= 0b11111111;  	// Inicializa��o dos canais anal�gicos do PORTA.
    TRISB	= 0b00000000;	// Configura��o das entradas/sa�das do PORTB.
    PORTB	= 0b00000000;  	// Inicializa��o das entradas/sa�das do PORTB.
	TRISC	= 0b10000000;	// Configura pino RC7(pino RX) como entrada, demais pinos como sa�das.
    PORTC	= 0b10000000;  	// Inicializa��o das entradas/sa�das do PORTC.
    TRISD	= 0b00000000;	// Configura��o das entradas/sa�das do PORTD.		
    PORTD	= 0b00000000;  	// Inicializa��o das das entradas/sa�das do PORTD.
    TRISE	= 0b11111111;	// Configura��o dos canais anal�gicos do PORTE.
    PORTE	= 0b11111111;  	// Inicializa��o dos canais anal�gicos do PORTE.

	USART_Init(110000,1);	// Inicializa��o da USART passando a velocidade e o modo de transmiss�o.
	ADC_Init();				// Inicializa��o do m�dulo do conversor A/D.
	
	INTCONbits.PEIE	= 1;	// Habilita Interrup��o de Perif�ricos do Microcontrolador.
	INTCONbits.GIE	= 1;	// Habilita Interrup��o Global.

	while(1)
	{
		
	}

}
//-----------------------------------------------------------------------------