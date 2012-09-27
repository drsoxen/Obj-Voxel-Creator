float4x4 World;
float4x4 View;
float4x4 Projection;

struct VertexShaderInput
{
    float4 Position : POSITION0;
	float4 Color : COLOR0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float4 Color : COLOR0;
};

BlendState AlphaBlending
{
	AlphaToCoverageEnable = FALSE;
	BlendEnable[0] = TRUE;
	SrcBlend = SRC_ALPHA;
	DestBlend = INV_SRC_ALPHA;
	BlendOp = ADD;
	SrcBlendAlpha = ONE;
	DestBlendAlpha = ONE;
	BlendOpAlpha = ADD;
	RenderTargetWriteMask[0] = 0x0F;
};

BlendState NoBlending
{
	AlphaToCoverageEnable = FALSE;
	BlendEnable[0] = FALSE;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
	output.Color = input.Color;
    output.Position = mul(viewPosition, Projection);

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float4 c= input.Color;
	c.a = 0.5;
    return c;
}

VertexShaderOutput wire_VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
	output.Color = input.Color;
    output.Position = mul(viewPosition, Projection);

    return output;
}

float4 wire_PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    return float4(0,0,0,1);
}

technique Technique1
{
    pass Pass0
    {
		SetVertexShader(CompileShader(vs_2_0, wire_VertexShaderFunction()));        
        SetPixelShader(CompileShader(ps_2_0, wire_PixelShaderFunction()));
		//SetBlendState( NoBlending, float4( 0.0f, 0.0f, 0.0f, 0.0f ), 0xFFFFFFFF );
    }

    pass Pass1
    {
		SetVertexShader(CompileShader(vs_2_0, VertexShaderFunction()));        
        SetPixelShader(CompileShader(ps_2_0, PixelShaderFunction()));
    }
}
