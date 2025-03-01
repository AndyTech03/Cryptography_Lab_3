using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cryptography_Lab_3
{
	public partial class Form1 : Form
	{
		// private BigInteger p_value =		BigInteger.Parse("179769313486231590772930519078902473361797697894230657273430081157732675805500963132708477322407536021120113879871393357658789768814416622492847430639474124377767893424865485276302219601246094119453082952085005768838150682342462881473913110540827237163350510684586298239947245938479716304835356329624224137859");
		// private BigInteger q_value =		BigInteger.Parse("179769313486231590772930519078902473361797697894230657273430081157732675805500963132708477322407536021120113879871393357658789768814416622492847430639474124377767893424865485276302219601246094119453082952085005768838150682342462881473913110540827237163350510684586298239947245938479716304835356329624224137037");
		private BigInteger e_key =			BigInteger.Parse("179769313486231590772930519078902473361797697894230657273430081157732675805500963132708477322407536021120113879871393357658789768814416622492847430639474124377767893424865485276302219601246094119453082952085005768838150682342462881473913110540827237163350510684586298239947245938479716304835356329624224138297");
		/*p_value * q_value*/
		private BigInteger n_key =			BigInteger.Parse("32317006071311007300714876688669951960444102669715484032130345427524655138867890893197201411522913463688717960921898019494119559150490921095088152386448283120630877367300996091750197750389652106796057638384067568276792218642619756161838094338476170470581645852036305042887575891541065808607552399123930385605327294847279800539324735639175317134730307858249347032949363316518987766460907207726992070461273830525676443978788375311276803882190958741084905429949280942511428233848405377929386996705118994895909032348099374675947913875255775642111208457333275379338239093081271987980840594059540513637161190556699595783783");
		/*(p_value - 1) * (q_value - 1)*/
		//private BigInteger pfi_n_value =	BigInteger.Parse("32317006071311007300714876688669951960444102669715484032130345427524655138867890893197201411522913463688717960921898019494119559150490921095088152386448283120630877367300996091750197750389652106796057638384067568276792218642619756161838094338476170470581645852036305042887575891541065808607552399123930385604967756220307337357778874601017512188006712462460885718402503154203522414849905281461575115816458758483436216219045588595959224344562125496099210568670332693755892446998674407376782557502626806657002866443929363138271612510570849879163382236251620905011538071712099391500946102182581081027490477897451147508888");
		private BigInteger d_key =			BigInteger.Parse("11422942154560588659501499580996999832096336771239332554253280597980011517195231536894986257416014697727759059016958207179885559709699444924025286121057196906849725884358245537256386069588068942985357815524521134566680112890879260788330160207132081874762556429188599719365898746653561438992534733956632355059909508214029873824517113906281088242937300859397639858403008281123518552371784353864407305512754239817038628169932366463985410804844560294972544367352919870598026394116800303299686482881173036981692384333342034885035335251888576741969794178674552996550222285671001818074896961071359072916446946985553703915769");
		private BigInteger h_init =			BigInteger.Parse("3686572480992542324203469940626698007373988844973414619718244494731087636787746775552523426118076837152887607451006340032185803915250427969092824041300926120435472254140368495370100395825331521554270791266298904881403754308992035851223930755200355079155892048689269739484215006444230002809097726053015908191421217921968545269858438075623498321762563089702654761892181924331786225718547551124843499785564916832502796920052537033705691969010527753938793645747647122254313192136009783252792483089030967703891637959418756715149547705882285905278871956024161520807354171029118625594886103630919568291555509088345972183861");
		
		public Form1()
		{
			InitializeComponent();
		}

		private BigInteger Encript(BigInteger m_value)
		{
			return BigInteger.ModPow(m_value, e_key, n_key);
		}

		private BigInteger Decript(BigInteger c_value)
		{
			return BigInteger.ModPow(c_value, d_key, n_key);
		}

		private BigInteger Sign(BigInteger m_value)
		{
			return BigInteger.ModPow(m_value, d_key, n_key);
		}

		private BigInteger TestSign(BigInteger s_value)
		{
			return BigInteger.ModPow(s_value, e_key, n_key);
		}

		private void EncodeB_Click(object sender, EventArgs e)
		{
			string sourceMessage = MessageRTB.Text;
			string digestString = "";
			var underlineFont = new Font(MessageRTB.Font, FontStyle.Underline);
			foreach (var match in GetDigestMathces(sourceMessage))
			{
				digestString += match.Groups[1];

				int start = match.Index;
				int len = match.Length;
				int aIndex = match.Value.IndexOf("а", StringComparison.OrdinalIgnoreCase);

				MessageRTB.Select(start, len);
				MessageRTB.SelectionBackColor = Color.Yellow;
				MessageRTB.Select(start + 1, 1);
				MessageRTB.SelectionBackColor = Color.Red;
				MessageRTB.Select(start + aIndex, 1);
				MessageRTB.SelectionColor = Color.Green;
				MessageRTB.SelectionFont = underlineFont;
			}
			MessageRTB.Update();
			MessageRTB.Invalidate();

			BigInteger hashCode = GetDigestHashCode(digestString);
			BigInteger sign = Sign(hashCode);

			string encodedMessage = GammaEncode(sourceMessage, hashCode, sign);
			EncodedMessageRTB.Text = encodedMessage;

			//BigInteger m_value = BigInteger.Parse(MessageRTB.Text);
			//BigInteger c_value = Encript(m_value);
			//EncodedMessageRTB.Text = c_value.ToString();
		}

		private void DecodeB_Click(object sender, EventArgs e)
		{
			string encodedMessage = EncodedMessageRTB.Text;
			string decodedMessage = GammaDecode(encodedMessage);
			MessageRTB.Text = decodedMessage;
			//BigInteger c_value = BigInteger.Parse(EncodedMessageRTB.Text);
			//BigInteger m_value = Decript(c_value);
			//MessageRTB.Text = m_value.ToString();
		}

		private IEnumerable<Match> GetDigestMathces(string souceMessage)
		{
			// 7	Каждый второй символ слов текста М, содержащих букву А
			var matches = Regex.Matches(souceMessage, @"(?i)(?=[а-я]*а)[а-я]([а-я])[а-я]*");
			foreach (Match match in matches)
				yield return match;
		}

		private BigInteger GetDigestHashCode(string digestString)
		{
			BigInteger hashCode = h_init;
			foreach (char c in digestString)
				hashCode = BigInteger.ModPow(hashCode + (int)c, 2, n_key);
			return hashCode;
		}

		private BigInteger GetMessageHashCode(string sourceMessage)
		{
			string digestString = "";
			foreach (var match in GetDigestMathces(sourceMessage))
				digestString += match.Groups[1];
			return GetDigestHashCode(digestString);
		}

		private string GammaEncode(string souceMessage, BigInteger hash, BigInteger sign)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(souceMessage);
			List<byte> result = new List<byte>();
			int maxBlockSize = 2048 / 8;
			// Кодирование сообщения
			for (int i = 0; i < bytes.Length; i+= maxBlockSize)
			{
				int blockSize = Math.Min(bytes.Length - i, maxBlockSize);
				byte[] block = new byte[blockSize];
				Array.Copy(bytes, i, block, 0, blockSize);
				BigInteger value = new BigInteger(block);

				BigInteger encodedValue = Encript(value);
				byte[] encodedBytes = encodedValue.ToByteArray();
				BigInteger lenValue = encodedBytes.Length;
				byte[] lenBytes = lenValue.ToByteArray();
				result.AddRange(lenBytes.Concat(encodedBytes));
			}
			// Якорь-разделитель
			{
				byte[] anchor = new byte[2] { 255, 255 };
				result.AddRange(anchor);
			}
			// Подставляем sign
			{
				byte[] encodedBytes = sign.ToByteArray();
				BigInteger lenValue = encodedBytes.Length;
				byte[] lenBytes = lenValue.ToByteArray();
				result.AddRange(lenBytes.Concat(encodedBytes));
			}

			return Convert.ToBase64String(result.ToArray());
		}

		private string GammaDecode(string encodedMessage)
		{
			byte[] bytes = Convert.FromBase64String(encodedMessage);
			List<byte> result = new List<byte>();
			BigInteger sign = BigInteger.Zero;
			bool isSign = false;
			// Декодирование сообщения
			for (int i = 0; i < bytes.Length;)
			{
				byte[] lenBytes = new byte[2];
				Array.Copy(bytes, i, lenBytes, 0, 2);
				int lenValue = (int)new BigInteger(lenBytes);
				// Переключение по достижению якоря
				if (lenValue == -1)
				{
					isSign = true;
					i += 2;
					Array.Copy(bytes, i, lenBytes, 0, 2);
					lenValue = (int)new BigInteger(lenBytes);
				}
				byte[] encodedBytes = new byte[lenValue];
				Array.Copy(bytes, i + 2, encodedBytes, 0, lenValue);

				BigInteger value = new BigInteger(encodedBytes);
				// Считываем подпись
				if (isSign) {
					sign = value;
					break;
				}
				BigInteger decriptedValue = Decript(value);
				result.AddRange(decriptedValue.ToByteArray());
				i += 2 + lenValue;
			}
			string message = Encoding.UTF8.GetString(result.ToArray());
			BigInteger hashCode = GetMessageHashCode(message);
			if (hashCode.Equals(TestSign(sign))) 
			{
				MessageBox.Show("Подпись подленая!");
			}
			else
			{
				MessageBox.Show("Подпись поддельная!");
			}
			return message;
		}
	}
}
