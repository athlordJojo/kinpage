sampler firstSampler;
float greyPower;

float4 GreyShader(float2 texCoord: TEXCOORD0) : COLOR
{
   float4 color = tex2D(firstSampler, texCoord);
   /*if(greyPower != -1){
	   float3 greyVek =  float3(0.3f, 0.59f, 0.11f)*greyPower;
	   color.rgb = dot(color.rgb, greyVek);
   }*/
   return color;
} 


float bloodpower;
//Umso starker bloodppower--> umso mehr rot
float4 BloodScreenShader(float2 texCoord: TEXCOORD0):COLOR
{
	float4 color = tex2D(firstSampler, texCoord);
	if(bloodpower != -1){
	  float r_t = (1.0f - color.r) * bloodpower;//diifferenz zum maximalen rotwert berechnen. diesen mit bloodpower mult und auf den r-wert raufaddieren
	  color.r += r_t;

	  color.g = color.g * bloodpower;
	  color.b = color.b * bloodpower;
   }

   return color;
}


float2 targetSize; 
float4 BlurShader(float2 texCoord: TEXCOORD0) : COLOR
{
	float2 invSize = (1.0f / (targetSize));

	float4 color = 0;
	color += tex2D(firstSampler, texCoord + float2(invSize.x, 0));
	color += tex2D(firstSampler, texCoord + float2(invSize.x * 3.0f, 0));
	color += tex2D(firstSampler, texCoord + float2(-invSize.x, 0));
	color += tex2D(firstSampler, texCoord + float2(-invSize.x * 3.0f, 0));
	color /= 4.0f; 
	return color;
} 

technique LangweiligerShader
{
   pass pass0
   {
      PixelShader = compile ps_2_0 GreyShader();
   }

    pass pass1
   {
      PixelShader = compile ps_2_0 BlurShader();
   }

   pass pass2{
	PixelShader = compile ps_2_0 BloodScreenShader();
   }
} 