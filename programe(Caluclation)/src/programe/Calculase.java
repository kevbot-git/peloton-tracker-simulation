/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package programe;

import static java.lang.Math.cos;

/**
 *
 * @author Alex
 */
public class Calculase {
           
    private Point A1;
    private Point B1;
    private Point C1;
    private Point Min;
    private Point B2;
    private Point C2;
    private Point Z1;
    private Point Z2;
    private Point ZZ1;
    private Point inA1B1;
   
    
    
    public Calculase(){
        
    }
    
    
    public Point CalculaseZ(Point A, Point B, Point C, int lenAZ, int lenBZ, int lenCZ){
        
         A1=new Point();
         B1=new Point();
         C1=new Point();
         Min=new Point();
         B2=new Point();
         C2=new Point();
         Z1=new Point();
         Z2=new Point();
         ZZ1=new Point();
         int x;
         int y;
         int upDown = 10;
         inA1B1=new Point();
            //Set smallest X value point to                                                                                                                                                                                                                                                                                            //set nearest point to A 
            if (A.getX() <= B.getX() && A.getX() <= C.getX())
            { A1 = A;
                if(B.getX()<=C.getX()){
                    B1= B;
                    C1= C;
                }else{
                    B1=C;
                    C1=B;
                } 
            }
            else if (B.getX() <= A.getX() && B.getX() <= C.getX())
            { A1 = B; 
                if(A.getX()<=C.getX()){
                    B1 =A ; C1= C;
                } else{
                    B1=C;
                    C1=A;
                }
            }else
             { A1 = C;
                if(A.getX()<=C.getX()){
                    B1 =A ; C1 =B; 
                }else{
                    B1=B;
                    C1=A;
                }
             }          
            //define A is on the 0 point lane left or right
          
            x=A1.getX();
            y=A1.getY();
            Min.setX(x);
            Min.setY(y);
            
            //move A B C to the 0 point
            A1.setX(0);
            A1.setY(0);
            B1.setX(B1.getX()-Min.getX());
            B1.setY(B1.getY()-Min.getY());
            C1.setX(C1.getX()-Min.getX());
            C1.setY(C1.getY()-Min.getY());
            //calculate length of AB,AC,BC
            int lenAB =(int)Math.sqrt(Math.pow(A.getX() - B.getX(), 2) + Math.pow(A.getY() -B.getY(), 2));
            int lenAC =(int)Math.sqrt(Math.pow(A.getX() - C.getX(), 2) + Math.pow(A.getY() -C.getY(), 2));
            int lenBC =(int)Math.sqrt(Math.pow(B.getX() - C.getX(), 2) + Math.pow(B.getY() -C.getY(), 2));
            System.out.println("AB="+lenAB+", AC="+lenAC+",BC="+lenBC);
            //Set New B point loaction
            B2.setX(lenAB);
            B2.setY(0);
            //calculate new C location
            C2.setX(((lenAC*lenAC)-(lenBC*lenBC)+(lenAB*lenAB))/(2*lenAB));
            if(B1.getY()>A1.getY()){
               C2.setY(-(int)Math.sqrt(lenAC*lenAC-C2.getX()*C2.getX()));
               System.out.println("X="+C2.getX()+",Y="+C2.getY()+",DOWN");
               upDown=-1;
            }
            if(B1.getY()<A1.getY()){
               C2.setY((int)Math.sqrt(lenAC*lenAC-C2.getX()*C2.getX()));
               System.out.println("X="+C2.getX()+",Y="+C2.getY()+",UP");
               upDown=1;
            }
            if(B1.getY()==A1.getX()){
                if(C1.getY()>A1.getY()){
                    C2.setY((int)Math.sqrt(lenAC*lenAC-C2.getX()*C2.getX()));
                    System.out.println("X="+C2.getX()+",Y="+C2.getY()+",NOT MOVE");
                    upDown=0;
                }else{
                    C2.setY(-(int)Math.sqrt(lenAC*lenAC-C2.getX()*C2.getX()));
                    System.out.println("X="+C2.getX()+",Y="+C2.getY()+",NOT MOVE");
                    upDown=0;
                }
            }
            //calculate Z2 location
            Z2.setX((lenAZ * lenAZ - lenBZ * lenBZ+ lenAB * lenAB) / (2 * lenAB));
            Z2.setY((lenAZ * lenAZ - lenCZ * lenCZ - Z2.getX() * Z2.getX() +(int)Math.pow(Z2.getX() -C1.getX(), 2)+C1.getY()*C1.getY())/(2*C1.getY()));
            System.out.println("X!!!!!!="+Z2.getX()+",Y="+Z2.getY());
            //calculate Z1 location
             float SinC= ((float)Math.abs(B1.getY())/((float)lenAB));
             int angle = (int)(Math.asin(SinC)*(180/Math.PI));
             System.out.println("angle="+angle);
             if(upDown == 1){
                 System.out.println("up");
                 Z1.setX((int)(Z2.getX()+lenAZ*Math.cos(angle*Math.PI/180))+Min.getX());
                 Z1.setY((int)(Z2.getY()+lenAZ*Math.sin(angle*Math.PI/180))+Min.getX());
                 System.out.println("Z:x="+Z1.getX()+",y="+Z1.getY());
             }if(upDown== -1){
                 System.out.println("down");
                 Z1.setX((int)(Z2.getX()+lenAZ*Math.cos((180-angle)*Math.PI/180))+Min.getX());
                 Z1.setY((int)(Z2.getY()+lenAZ*Math.sin((180-angle)*Math.PI/180))+Min.getX());
                 System.out.println("Z:x="+Z1.getX()+",y="+Z1.getY());
             }if(upDown== 0){
                 Z1=Z2;
             }if(upDown==10){
                 System.out.println("not work");
             }
            
//              x1   =   x0   +   r   *   cos(ao   *   3.14   /180   ) 
//              y1   =   y0   +   r   *   sin(ao   *   3.14   /180   ) 
//            float tanBAB = (float)B1.getY() / (float)B1.getX();
//            float tanCAC = (float)Z2.getY() / (float)Z2.getX();
//            float tanCAB2;
//            if (B1.getY() >= 0)
//                tanCAB2 = (tanBAB + tanCAC) / (1 - tanBAB * tanCAC);
//            else
//                tanCAB2 = (tanCAC - tanBAB) / (1 + tanCAC * tanBAB);
//            Z1.setX((int)((float)lenAZ / Math.sqrt(tanCAB2 * tanCAB2 + 1)));
//            Z1.setY((int)(tanCAB2 * Z1.getX()));
//            //calculate ZZ1 point location
//            inA1B1.setX((A1.getX()+B1.getX())/2);
//            inA1B1.setY((A1.getY()+B1.getY())/2);
//            ZZ1.setX(inA1B1.getX() * 2 - Z1.getX());
//            ZZ1.setY(inA1B1.getY()*2-Z1.getY());
//            //comper C1Z1.C1ZZ1. which is near CZ. and this point is Z1
//
//
//            int lenZZ1C1 = (int)Math.sqrt(Math.pow(ZZ1.getX() - C1.getX(), 2) + Math.pow(ZZ1.getY() - C1.getY(), 2));
//            int lenZ1C1 = (int)Math.sqrt(Math.pow(Z1.getX() - C1.getX(), 2) + Math.pow(Z1.getY() - C1.getY(), 2));
//            
//            if ((lenZZ1C1 - lenCZ) < (lenZ1C1 - lenCZ)){
//                Z1 = ZZ1;
//            }else{
//                
//            }
//            //get Z loaction
//            Z1.setX(Z1.getX()+Min.getX());
//            Z1.setY(Z1.getY()+Min.getY());
//
            return Z1;
    }
   
}
