 
Shader "Terrain3L" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,1)  
        _Shininess ("Shininess", Range (0.01, 1)) = 0.7
 
        _PathTex ("Path Texture (RGB) Mask (A)", 2D) = "black"  
        _PathMaskTex ("Path Mask (A)", 2D) = "black"
       
       
       
    }
    SubShader {  
     
    Pass{        
 
             Material {
            Diffuse (1,1,1,1)
          }                        
           Lighting On    
          SetTexture [_PathTex] { combine texture * previous  }    
         SetTexture [_PathMaskTex] {  Combine texture lerp (texture) previous   }
         
       
   
 
     }   // end pass    // end pass
    }
   FallBack " Diffuse", 1
}