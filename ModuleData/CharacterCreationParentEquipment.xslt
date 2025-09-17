<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output omit-xml-declaration="yes"/>
	<xsl:template match="@*|node()">
		<xsl:copy>
			<xsl:apply-templates select="@*|node()"/>
		</xsl:copy>
	</xsl:template>
	<!-- aserai -->
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_aserai_1_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_aserai_1_f']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_aserai_2_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_aserai_2_f']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_aserai_3_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_aserai_3_f']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_aserai_4_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_aserai_4_f']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_aserai_5_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_aserai_5_f']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_aserai_6_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_aserai_6_f']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_aserai_9_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_aserai_9_f']"/>
	<!-- battania -->
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_battania_1_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_battania_1_f']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_battania_2_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_battania_2_f']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_battania_3_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_battania_3_f']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_battania_4_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_battania_4_f']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_battania_5_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_battania_5_f']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_battania_8_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_battania_8_f']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_battania_6_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_battania_6_f']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_battania_9_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_battania_9_f']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_battania_10_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_battania_10_f']"/>
	<!-- empire -->
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_empire_1_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_empire_1_f']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_empire_2_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_empire_2_f']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_empire_3_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_empire_3_f']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_empire_4_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_empire_4_f']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_empire_5_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_empire_5_f']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_empire_6_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_empire_6_f']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_empire_9_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_empire_9_f']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_empire_10_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_empire_10_f']"/>
	<!-- khuzait -->
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_khuzait_1_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_khuzait_1_f']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_khuzait_2_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_khuzait_2_f']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_khuzait_3_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_khuzait_3_f']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_khuzait_4_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_khuzait_4_f']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_khuzait_5_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_khuzait_5_f']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_khuzait_6_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_khuzait_6_f']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_khuzait_9_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_khuzait_9_f']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_khuzait_10_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_khuzait_10_f']"/>
	<!-- sturgia -->
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_sturgia_1_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_sturgia_1_f']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_sturgia_2_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_sturgia_2_f']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_sturgia_3_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_sturgia_3_f']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_sturgia_4_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_sturgia_4_f']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_sturgia_5_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_sturgia_5_f']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_sturgia_6_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_sturgia_6_f']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_sturgia_9_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_sturgia_9_f']"/>
	<!-- vlandia -->
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_vlandia_1_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_vlandia_1_f']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_vlandia_2_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_vlandia_2_f']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_vlandia_3_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_vlandia_3_f']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_vlandia_4_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_vlandia_4_f']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_vlandia_5_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_vlandia_5_f']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_vlandia_6_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_vlandia_6_f']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_vlandia_9_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_vlandia_9_f']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_vlandia_10_m']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_vlandia_10_f']"/>
    
  	<!-- CHARACTER CREATION SHOW ARMOR SET -->
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_show_aserai']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_show_battania']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_show_empire']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_show_khuzait']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_show_sturgia']"/>
	<xsl:template match="EquipmentRoster[@id='player_char_creation_childhood_age_show_vlandia']"/>
</xsl:stylesheet>
