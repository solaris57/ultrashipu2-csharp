> **Documentación disponible en español más abajo**
# Ultraship U2 Technical documentation for C# - ENGLISH

## 00. About the author and project
---
Hello, my name is Solaris and I'm not a programming expert. I created this project as a hobby and to hopefully help others save time during their research. If you have any questions or feedback about this project, feel free to reach out to me on Discord at solaris#7753.

This documentation is intended to be used on a Windows system, and to be a starting point or a guide to a bigger project.

The source code is intended to be edited and compiled to meet the user's requirements.

## 01. Serial port
---
To connect the scale, it's necessary to configure the serial port with these parameters that are needed for the serial communication:

1. **Interface:** The scale uses a USB port and will appear as a COM port on your system. You'll need to identify which port it's connected to and set this value accordingly in your code. In my case, the scale is connected to COM4.

2. **Baud rate:** The default baud rate for the scale is 9600, which is the speed at which data is transmitted over the serial connection.

3. **Parity bits:** Specifies whether a parity bit is used for error checking during transmission. The Ultraship U2 does not require any.

4. **Data bits:** Specifies the number of bits used for each character in the data stream. The cale uses 8 data bits.


The definition of the serial port should look like this:
```csharp
SerialPort port = new SerialPort("COM4", 9600, Parity.None, 8, StopBits.One);
```
It's also reccomended to add a timeout, however, it's not required.
```csharp
port.ReadTimeout = 1000;
```
Finnaly, the port is configured and can be opened to start the communication.
```csharp
port.Open();
```

## 02. Data reading
---
The scale's SEND button sends raw binary data into the computer, which then can be converted into hex or ASCII. 
```csharp
byte[] buffer = new byte[10];
int bytesRead = 0;
```
Once the 10 byte-long buffer is set, it's ready to be read, by using a simple while loop and try catch it can be done easily. Note that "0" is the starting point of the data store.

```csharp
  try
    {
        bytesRead = port.Read(buffer, 0, buffer.Length);}
    catch (TimeoutException)
    {
        Console.WriteLine("Waiting...");
    continue;
    }
```
## 03. Decoding
---
The scale sends different types of hex codes to represent its current status:
| Code | Status|
|-----------|--------------------------|
| FF        | Power button pressed     |
| FC        | Reboot (Power button held)|
| 00        | OFF                      |
| 01        | ON                       |
| 02        | SEND button pressed      |
| 0B-44     | Incoming weigh data      |


Since the scale can change the unit of measurement by pressing the M1 and M2 buttons, the data received by the computer when changing these values is reflected as follows: XX-XX-03, where the value 03 represents the end of the data transmission, and XX is replaced by the following values:

| Code | Measure unit|
|-----------|---------|
| 4B        | Kg|
| 47        | g|
| 50        | lb|
| 4F        | oz|
| 4C        | lb:oz|


> For example, if the program returns the string "4B-4F-03," it means that the weight on the scale is being represented as Kg to oz.


## 04. License
---
This project is released under the [MIT License](https://opensource.org/licenses/MIT), which means that anyone is free to use, modify, and distribute the source code without requiring permission from the author. However, it is provided as-is and without warranty, and the author shall not be held liable for any damages or losses incurred from its use. By using this project, you agree to the terms of the MIT License. 

----
# Documentación técnica de Ultraship U2 para C# - ESPAÑOL

## 00. Acerca del autor y del proyecto
---
Hola, mi nombre es Solaris y no soy una experta en programación. Creé este proyecto como un pasatiempo y con la esperanza de ayudar a otros a ahorrar tiempo durante su investigación. Si tienes alguna pregunta o comentario sobre este proyecto, no dudes en ponerte en contacto conmigo en Discord: solaris#7753.

Esta documentación está destinada a ser utilizada en un sistema Windows y como punto de partida o guía para un proyecto más grande.

El código fuente está pensado para ser editado y compilado según las necesidades del usuario.
## 01. Puerto serie
---
Para conectar la báscula, es necesario configurar el puerto serie con estos parámetros para la comunicación serie:

1. Interfaz: La báscula utiliza un puerto USB y aparecerá como un puerto COM en tu sistema. Deberás identificar a qué puerto está conectado y establecer este valor en el código. En mi caso, la báscula está conectada al puerto COM4.

2. Velocidad en baudios: La velocidad en baudios predeterminada para la báscula es 9600, que es la velocidad a la que se transmite la información a través de la conexión serie.

3. Bits de paridad: Especifica si se utiliza un bit de paridad para la detección de errores durante la transmisión. La Ultraship U2 no requiere ningún bit de paridad.

4. Bits de datos: Especifica el número de bits que se utilizan para cada carácter en la secuencia de datos. La báscula utiliza 8 bits de datos.


La definición del puerto serie debería parecerse a esto:
```csharp
SerialPort port = new SerialPort("COM4", 9600, Parity.None, 8, StopBits.One);
```
Puedes agregar un tiempo de espera, sin embargo, no es obligatorio.
```csharp
port.ReadTimeout = 1000;
```
Finalmente, el puerto está configurado y puede abrirse para iniciar la comunicación.
```csharp
port.Open();
```

## 02. Lectura de datos
---
El botón SEND de la báscula envía datos en binario sin procesar a la computadora, que luego se pueden convertir en hexadecimal o ASCII.
```csharp
byte[] buffer = new byte[10];
int bytesRead = 0;
```
Una vez que se establece el buffer de 10 bytes de longitud, los datos están listos para ser leídos. Esto se consigue fácilmente usando un simple bucle while y try catch. Ten en cuenta que "0" es el punto de inicio del almacenamiento de datos en el array.
```csharp
  try
    {
        bytesRead = port.Read(buffer, 0, buffer.Length);}
    catch (TimeoutException)
    {
        Console.WriteLine("Esperando..."); //Puedes escribir tu propio mensaje!
    continue;
    }
```
## 03. Decodificación
---
La báscula envía diferentes tipos de datos en hexadecimal para representar su estado actual:

| Código | Estado                              |
|--------|-------------------------------------|
| FF     | Botón de encendido presionado        |
| FC     | Reinicio (Botón de encendido mantenido) |
| 00     | APAGADO                             |
| 01     | ENCENDIDO                           |
| 02     | Botón SEND presionado               |
| 0B-44  | Entrada de datos de peso            |


Ya que la báscula puede cambiar la unidad de medida al presionar los botones M1 y M2, los datos que recibe el ordenador al cambiar estos valores se reflejan de la siguiente manera: XX-XX-03, en donde el valor 03 representa el final del envío de datos y XX se sustituye por los valores a continuación:

| Código | Unidad |
|--------|--------|
| 4B     | Kg     |
| 47     | g      |
| 50     | lb     |
| 4F     | oz     |
| 4C     | lb:oz  |

> Por ejemplo, si el programa devuelve la cadena 4B-4F-03, significa que el peso en la báscula se está representando como Kg a oz.

## 04. Licencia
---
Este proyecto se publica bajo la [Licencia MIT](https://opensource.org/licenses/MIT), lo que significa que cualquier persona es libre de usar, modificar y distribuir el código fuente sin necesidad de permiso del autor. Sin embargo, se proporciona "tal cual", sin garantía de ningún tipo, y el autor no será responsable de ningún daño o pérdida derivados de su uso. Al utilizar este proyecto, aceptas los términos de la Licencia MIT.
