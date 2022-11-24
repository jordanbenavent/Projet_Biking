
package com.soap.ws.client.generated;

import javax.xml.bind.JAXBElement;
import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlElementRef;
import javax.xml.bind.annotation.XmlType;


/**
 * <p>Classe Java pour CompositeType complex type.
 * 
 * <p>Le fragment de schéma suivant indique le contenu attendu figurant dans cette classe.
 * 
 * <pre>
 * &lt;complexType name="CompositeType"&gt;
 *   &lt;complexContent&gt;
 *     &lt;restriction base="{http://www.w3.org/2001/XMLSchema}anyType"&gt;
 *       &lt;sequence&gt;
 *         &lt;element name="BoolValue" type="{http://www.w3.org/2001/XMLSchema}boolean" minOccurs="0"/&gt;
 *         &lt;element name="StringValue" type="{http://www.w3.org/2001/XMLSchema}string" minOccurs="0"/&gt;
 *       &lt;/sequence&gt;
 *     &lt;/restriction&gt;
 *   &lt;/complexContent&gt;
 * &lt;/complexType&gt;
 * </pre>
 * 
 * 
 */
@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "CompositeType", namespace = "http://schemas.datacontract.org/2004/07/ServeurSoapBiking", propOrder = {
    "boolValue",
    "stringValue"
})
public class CompositeType {

    @XmlElement(name = "BoolValue")
    protected Boolean boolValue;
    @XmlElementRef(name = "StringValue", namespace = "http://schemas.datacontract.org/2004/07/ServeurSoapBiking", type = JAXBElement.class, required = false)
    protected JAXBElement<String> stringValue;

    /**
     * Obtient la valeur de la propriété boolValue.
     * 
     * @return
     *     possible object is
     *     {@link Boolean }
     *     
     */
    public Boolean isBoolValue() {
        return boolValue;
    }

    /**
     * Définit la valeur de la propriété boolValue.
     * 
     * @param value
     *     allowed object is
     *     {@link Boolean }
     *     
     */
    public void setBoolValue(Boolean value) {
        this.boolValue = value;
    }

    /**
     * Obtient la valeur de la propriété stringValue.
     * 
     * @return
     *     possible object is
     *     {@link JAXBElement }{@code <}{@link String }{@code >}
     *     
     */
    public JAXBElement<String> getStringValue() {
        return stringValue;
    }

    /**
     * Définit la valeur de la propriété stringValue.
     * 
     * @param value
     *     allowed object is
     *     {@link JAXBElement }{@code <}{@link String }{@code >}
     *     
     */
    public void setStringValue(JAXBElement<String> value) {
        this.stringValue = value;
    }

}
