/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package programe;



    class Program
    {
        
        
        public static void main(String[] args){
            
            Point a = new Point(-5,-5);
            int ax=-5,ay=-5,bx=47,by=25,cx=-10,cy=55;
            Point b = new Point(47,25);
            Point c = new Point(-10,45); 
            Point z;
            int az=34; 
            int bz=34;
            int cz=34;
            Calculase N=new Calculase();
            z =N.CalculaseZ(a, b, c, az, bz, cz);
            System.out.println("Z:x="+z.getX()+",y="+z.getY());
            System.out.println((int)Math.sqrt(Math.pow(ax-z.getX(), 2) + Math.pow(ay-z.getY(), 2)));
            System.out.println((int)Math.sqrt(Math.pow(bx-z.getX(), 2) + Math.pow(by-z.getY(), 2)));
            System.out.println((int)Math.sqrt(Math.pow(cx-z.getX(), 2) + Math.pow(cy-z.getY(), 2)));
            System.out.println("C:x="+c.getX()+",y="+c.getY());
            System.out.println((int)Math.sqrt(Math.pow(7, 2) + Math.pow(-23, 2)));
            System.out.println(Math.asin(0.5)*(180/Math.PI));
        }
    }

