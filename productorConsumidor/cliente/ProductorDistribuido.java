/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package productordistribuido;

import java.io.BufferedReader;
import java.io.DataOutput;
import java.io.DataOutputStream;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.Socket;

/**
 *
 * @author Eduardo
 */
public class ProductorDistribuido {

    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
        // TODO code application logic here
        int puerto = 3000;
        DataOutputStream mensaje;


        try{
            Socket socket= new Socket ("127.0.0.1", puerto);
            mensaje = new DataOutputStream(socket.getOutputStream());
            mensaje.writeUTF("agregar");
            socket.close();
        } catch (IOException e)
        {
            System.out.println("Error en conexión al intentar con el puerto " + puerto  + "!!!");
        }
    }
    
}
