# Evidencija Pacijenata
Projekat .NET - Evidencija pacijenata i zakazivanje pregleda kod izabranog lekara

Zadatak: Постоји четири врсте корисника: администратор, лекари опште праксе, лекари специјалисти и пацијенти. Треба омогућити пријављивање корисника на систем. Корисник има могућност да након исправно унетих података настави рад са остатком система. У случају погрешно унетих података приказује му се одговарајућа порука. Поред пријављивања на систем, кориснику на почетном екрану треба обезбедити промену лозинке и регистрацију, у случају да је корисник у систему нови. Регистрација треба да омогући унос следећих података:
•	Име,
•	Презиме,
•	Пол,
•	Број здравствене књижице (корисничко име),
•	Лозинка,
•	Потврда лозинке,
•	ЈМБГ,
•	Крвна група,
•	Име и презиме носиоца осигурања,
•	Сродство са носиоцем осигурања,
•	Адреса (улица и број, град),
•	Контакт телефон,
•	Електронска пошта.
Ако су подаци исправно унети (извршити неке основне провере) треба креирати нови захтев за регистрацију. Администратор је задужен за разматрање пристиглих регистрација, а исход може да буде прихватање захтева или одбацивање захтева. Ако администратор прихвати захтев, сматра  се да је корисник платио здравствено осигурање и додељује му се здравствена установа (којој припада). Код промене шифре потребно је да осим уноса броја здравствене књижице и шифре, корисник унесе и нову шифру. Ако број здравствене књижице не постоји или шифра није добра потребно је приказати одговарајућу поруку. Када се успешно промени шифра, вратити се на  екран за пријављивање на систем.
Лекар опште праксе
Лекар опште праксе је корисник који припада некој здравственој установи и неком одељењу те установе. Након успешног пријављивања на систем, лекар опште праксе може да:
•	Врши претрагу пацијената по имену и презимену, и прегледа историју лечења одређеног пацијента. Преглед историје лечења може да се уради и уносом ЈМБГ пацијента. Лекару опште праксе се приказују само пацијенти који припадају установи којој припада и лекар.
•	Напише извештај о обављеном прегледу (извештај треба да садржи идентификацију пацијента, датум прегледа, дијагнозу - од чега болује пацијент, назив болести, тегобе, прописану терапију - лекове и датум и напомену за следеће контроле).
•	Напише упут лекару специјалисти (упут треба да садржи идентификацију пацијента, датум прегледа и установа у коју се упућује пацијент); лекар специјалиста представља лекара који је стручњак за одређену врсту болести или болести одређеног органа (интерниста-кардиолог, ортопед, радиолог, нефролог, реуматолог, офталмолог, биохемичар...); листа установа у коју се упућује пацијент, треба да буде јединствена на  нивоу Републике Србије.
•	ажурира тренутне картоне, односно допуни их одређеним налазом, који садржи:
o	датум и време налаза
o	дисање (10-90)
o	пулс (60-160)
o	телесна  температура (36-41)
o	крвни притисак (систолни/дијастолни)
o	мокраћа - у реду / није у реду
o	столица - у реду / није у реду
o	крвна слика - у реду / није у реду
o	подаци о специфичном прегледу (ултразвук/рендген)
Лекар специјалиста
Лекар специјалиста је корисник који припада некој здравственој установи и неком одељењу те установе. Након успешног пријављивања на систем, лекар специјалиста може да:
•	напише извештај лекара специјалисте (извештај треба да садржи идентификацију пацијента, датум прегледа, дијагнозу - од чега болује пацијент, налаз и закључак); на основу закључка, лекар специјалиста може да напише упут другом лекару, да задржи пацијента на одељењу или да га пошаље на кућно лечење, уз прописану котролу;
•	претражује пацијенте смештене на његовом одељењу и прегледа тренутне картоне (податке о пацијентима, који су тренутно примљени на том одељењу и њихове прегледе);
•	ажурира тренутне картоне, односно допуни их одређеним налазом, који садржи:
o	датум и време налаза
o	дисање (10-90)
o	пулс (60-160)
o	телесна  температура (36-41)
o	крвни притисак (систолни/дијастолни)
o	мокраћа - у реду / није у реду
o	столица - у реду / није у реду
o	крвна слика - у реду / није у реду
o	подаци о специфичном прегледу (ултразвук/рендген)
Пацијент
Пацијент, који је платио осигурање, када се успешно улогује на систем, има могућност прегледа свог картона и историје свог лечења. Уколико пацијент није платио осигурање, пацијенту је забрањен приступ систему, све док не продужи осигурање. Поред тога, пацијент има могућност заказивања прегледа код лекара опште праксе у дому здравља ком припада. Приликом заказивања, осим одабира лекара, потребно је да одабере и датум и време прегледа.
Администратор
Администратор је корисник са посебним привилегијама. Он може у систем да уноси нове, ажурира и/или брише постојеће установе, одељења, лекаре опште праксе, лекаре специјалисте и вести.
Остале карактеристике апликације
Потребно је направити и униформни изглед апликације користећи CSS - Cascading Style Sheets. Свака страница треба да садржи мени и заглавља (header и footer). Пожељно је да апликација буде прилагођена и другим платформама - responsive design (нпр. Андроид). На свим екранима где је приказан жељени садржај треба омогућити опцију за повратак на почетни екран са корисничким опцијама. Такође на свим екранима је потребан и линк који води на почетни екран за пријављивање и одјављивање (опција: Излогуј се).
У формама за унос извршити потребне валидације података коришћењем JavaScript-а.
