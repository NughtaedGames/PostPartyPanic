Shader "Unlit/mask"
{
	Properties{}

		SubShader{

			Tags {
				"RenderType" = "Opaque"
			}

			Pass {
				ZWrite Off
			}
	}
}