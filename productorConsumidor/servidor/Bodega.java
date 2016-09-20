/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package productorconsumidordistribuido;

/**
 *
 * @author Eduardo
 */
public class Bodega {
    private int productos = 0;
    private int CAPACIDAD_MAX = 10;

    /**
     * @return the productos
     */
    public int getProductos() {
        return productos;
    }

    /**
     * @param productos the productos to set
     */
    public void setProductos(int productos) {
        this.productos = productos;
    }

    /**
     * @return the CAPACIDAD_MAX
     */
    public int getCAPACIDAD_MAX() {
        return CAPACIDAD_MAX;
    }
}
