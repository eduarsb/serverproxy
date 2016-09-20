/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package productorconsumidordistribuido;

import java.io.BufferedReader;
import java.io.DataOutputStream;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.net.ServerSocket;
import java.net.Socket;

/**
 *
 * @author Eduardo
 */

public class ProductorConsumidorDistribuido {
    public static Bodega bodega;
    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) throws InterruptedException {
        // TODO code application logic here
        bodega = new Bodega();
        try {

            int puerto = 3000;

            ServerSocket s = new ServerSocket(puerto);
            System.out.println("Servidor iniciado en el puerto " + puerto + "...");	
            int i = 0;
            while(true){
                    System.out.println("*****Escuchando conexión.....***"); 
                    Socket s1 = s.accept();
                    System.out.println("*****Aceptando conexión de: " + s1.getInetAddress().getHostAddress() + "******");
                    BufferedReader entrada = new BufferedReader(new InputStreamReader(s1.getInputStream()));
                    DataOutputStream salida = new DataOutputStream(s1.getOutputStream());
                    System.out.println("Confirmando conexion al cliente....");
                    //salida.writeUTF("Conexión exitosa...n envia un mensaje :D");
                    String mensajeRecibido = entrada.readLine();
                    mensajeRecibido = mensajeRecibido.substring(2);
                    System.out.println(mensajeRecibido);
                    if(mensajeRecibido.equals("agregar")){
                        if (bodega.getProductos() < bodega.getCAPACIDAD_MAX()) {
                            bodega.setProductos(bodega.getProductos() + 1);
                        }
                    } else {
                        if (bodega.getProductos() > 0) {
                            bodega.setProductos(bodega.getProductos() - 1);
                        }
                    }
                    
            }
        } catch (IOException e) {
                e.printStackTrace();
                System.exit(-1);
        }
    }
    
}
