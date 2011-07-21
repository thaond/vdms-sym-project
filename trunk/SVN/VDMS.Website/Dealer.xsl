<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" indent="yes" encoding="utf-8"/>
	<!-- Find the root node called Menus 
       and call MenuListing for its children -->
	<xsl:template match="/Data">
		<MenuItems>
			<xsl:call-template name="MenuListing" />
		</MenuItems>
	</xsl:template>

	<!-- Allow for recusive child node processing -->
	<xsl:template name="MenuListing">
		<xsl:apply-templates select="Dealer" />
	</xsl:template>

	<xsl:template match="Dealer">
		<MenuItem>
			<!-- Convert Menu child elements to MenuItem attributes -->
			<xsl:attribute name="title">
				<xsl:value-of select="Name"/>
			</xsl:attribute>

			<!-- Call MenuListing if there are child Menu nodes -->
			<xsl:if test="count(Dealer) > 0">
				<xsl:call-template name="MenuListing" />
			</xsl:if>
		</MenuItem>
	</xsl:template>
</xsl:stylesheet>
